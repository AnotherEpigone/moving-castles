using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using MovingCastles.Serialization.Settings;
using SadConsole;
using SadConsole.Input;
using System.Collections.Generic;
using System.Linq;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace MovingCastles.Ui.Consoles
{
    internal class DungeonMapConsole : ContainerConsole
    {
        private readonly IMapModeMenuProvider _menuProvider;
        private readonly IAppSettings _appSettings;
        private readonly ITurnBasedGame _game;

        private Point _lastSummaryConsolePosition;
        private MouseHighlightConsole _mouseHighlight;
        private InteractTargettingConsole _interactTargettingConsole;

        public event System.EventHandler<ConsoleListEventArgs> SummaryConsolesChanged;
        public event System.EventHandler<string> FlavorMessageChanged;

        public Point _lastMousePos;
        public Point _targettedConsolePos;

        public DungeonMap Map { get; private set; }

        public ScrollingConsole MapRenderer { get; private set; }

        public Wizard Player { get; private set; }

        public DungeonMapConsole(
            int viewportWidth,
            int viewportHeight,
            Font tilesetFont,
            IMapModeMenuProvider menuProvider,
            ITurnBasedGame game,
            IAppSettings appSettings,
            DungeonMap map)
        {
            IsFocused = true;

            _menuProvider = menuProvider;
            _appSettings = appSettings;
            _game = game;

            _mouseHighlight = new MouseHighlightConsole(viewportWidth, viewportHeight, tilesetFont, game, map);
            _interactTargettingConsole = new InteractTargettingConsole(tilesetFont, map);

            Map = map;
            _game.Map = map;

            foreach (var entity in map.Entities.Items.OfType<McEntity>())
            {
                if (entity is Wizard player)
                {
                    Player = player;
                    _game.RegisterPlayer(player);
                    player.RemovedFromMap += Player_RemovedFromMap;
                    Player.Moved += Player_Moved;
                    continue;
                }

                _game.RegisterEntity(entity);
            }

            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, viewportWidth, viewportHeight), tilesetFont);
            MapRenderer.UseMouse = false;
            MapRenderer.CenterViewPortOnPoint(Player.Position);

            Map.CalculateFOV(Player.Position, Player.FovRadius, Radius.SQUARE);

            Children.Add(MapRenderer);
            Children.Add(_mouseHighlight);
            Children.Add(_interactTargettingConsole);
        }

        public void SetMap(DungeonMap map)
        {
            _mouseHighlight = new MouseHighlightConsole(_mouseHighlight.Width, _mouseHighlight.Height, _mouseHighlight.Font, _game, map);
            _interactTargettingConsole = new InteractTargettingConsole(_interactTargettingConsole.Font, map);

            foreach (var entity in Map.Entities.Items.OfType<McEntity>())
            {
                if (entity is Wizard player)
                {
                    player.RemovedFromMap -= Player_RemovedFromMap;
                    Player.Moved -= Player_Moved;
                    continue;
                }

                _game.UnregisterEntity(entity);
            }

            Map = map;
            _game.Map = map;

            foreach (var entity in map.Entities.Items.OfType<McEntity>())
            {
                if (entity is Wizard player)
                {
                    Player = player;
                    _game.RegisterPlayer(player);
                    player.RemovedFromMap += Player_RemovedFromMap;
                    Player.Moved += Player_Moved;
                    continue;
                }

                _game.RegisterEntity(entity);
            }

            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, MapRenderer.Width, MapRenderer.Height), MapRenderer.Font);
            MapRenderer.UseMouse = false;
            MapRenderer.CenterViewPortOnPoint(Player.Position);

            Map.CalculateFOV(Player.Position, Player.FovRadius, Radius.SQUARE);

            Children.Clear();
            Children.Add(MapRenderer);
            Children.Add(_mouseHighlight);
            Children.Add(_interactTargettingConsole);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (!Player.HasMap)
            {
                return base.ProcessKeyboard(info);
            }

            return _game.State switch
            {
                State.PlayerTurn => PlayerTurnProcessKeyboard(info),
                State.Processing => base.ProcessKeyboard(info),
                State.Targetting => TargettingProcessKeyboard(info),
                State.InteractTargetting => InteractTargettingProcessKeyboard(info),
                _ => base.ProcessKeyboard(info),
            };
        }

        public override bool ProcessMouse(MouseConsoleState state)
        {
            if (!Player.HasMap)
            {
                return base.ProcessMouse(state);
            }

            var mapState = new MouseConsoleState(MapRenderer, state.Mouse);

            var mouseMapCoord = new Coord(
                mapState.ConsoleCellPosition.X + MapRenderer.ViewPort.X,
                mapState.ConsoleCellPosition.Y + MapRenderer.ViewPort.Y);

            var coordIsTargetable = mapState.IsOnConsole && Map.FOV.CurrentFOV.Contains(mouseMapCoord);
            if (coordIsTargetable && _lastSummaryConsolePosition != mapState.ConsoleCellPosition)
            {
                // update summaries
                var summaryControls = new List<Console>();
                foreach (var entity in Map.GetEntities<BasicEntity>(mouseMapCoord))
                {
                    var control = entity.GetGoRogueComponent<ISummaryControlComponent>()?.GetSidebarSummary();
                    if (control != null)
                    {
                        summaryControls.Add(control);
                    }
                }

                _lastSummaryConsolePosition = mapState.ConsoleCellPosition;
                SummaryConsolesChanged?.Invoke(this, new ConsoleListEventArgs(summaryControls));
            }

            if (coordIsTargetable && _lastMousePos != mapState.ConsolePixelPosition)
            {
                _lastMousePos = mapState.ConsolePixelPosition;
                _targettedConsolePos = mapState.ConsoleCellPosition;
            }

            if (_targettedConsolePos == default
                || _game.State != State.Targetting)
            {
                _targettedConsolePos = mapState.ConsoleCellPosition;
            }

            var targetMapCoord = _targettedConsolePos + MapRenderer.ViewPort.Location;

            _mouseHighlight.IsVisible = mapState.IsOnConsole && Map.Explored[targetMapCoord];
            _mouseHighlight.Draw(_targettedConsolePos, MapRenderer.ViewPort.Location, Map.FOV.CurrentFOV.Contains(targetMapCoord));

            if (!_mouseHighlight.IsVisible && _lastSummaryConsolePosition != default)
            {
                // remove the summaries if we just moved out of a valid location
                _lastSummaryConsolePosition = default;
                SummaryConsolesChanged?.Invoke(this, new ConsoleListEventArgs(new List<Console>()));
            }

            if (coordIsTargetable && _game.State == State.Targetting)
            {
                TargettingProcessMouse(state, mouseMapCoord);
            }

            return base.ProcessMouse(state);
        }

        public override void Update(System.TimeSpan timeElapsed)
        {
            _interactTargettingConsole.IsVisible = _game.State == State.InteractTargetting;
            _interactTargettingConsole.Draw(Player.Position, _game.TargetInteractables);

            base.Update(timeElapsed);
        }

        private bool HandleGuiKeys(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.I))
            {
                EndTargettingMode();
                _menuProvider.Inventory.Show(Player.GetGoRogueComponent<IInventoryComponent>());
                return true;
            }

            if (info.IsKeyPressed(Keys.S))
            {
                EndTargettingMode();
                _menuProvider.SpellSelect.Show(
                    Player.GetGoRogueComponent<ISpellCastingComponent>().Spells,
                    selectedSpell => BeginTargetting(selectedSpell));

                return true;
            }

            if (info.IsKeyPressed(Keys.C))
            {
                EndTargettingMode();
                _menuProvider.Command.Show();
                return true;
            }

            if (info.IsKeyPressed(Keys.Escape)
                || info.IsKeyPressed(Keys.M))
            {
                _menuProvider.Pop.Show();
                return true;
            }

            return false;
        }

        private void TargettingProcessMouse(MouseConsoleState state, Coord mapCoord)
        {
            if (state.Mouse.LeftClicked)
            {
                var (success, target) = Map.GetTarget(Map.Player.Position, mapCoord, _game.TargettingSpell.TargettingStyle);
                if (success)
                {
                    _game.SpellTargetSelected(target);
                    EndTargettingMode();
                }
            }
        }

        private bool TargettingProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Escape))
            {
                EndTargettingMode();
                return true;
            }

            if (HandleGuiKeys(info))
            {
                return true;
            }

            if (info.IsKeyPressed(Keys.E)
                || info.IsKeyPressed(Keys.Enter))
            {
                var targetMapCoord = _targettedConsolePos + MapRenderer.ViewPort.Location;
                var (success, target) = Map.GetTarget(Map.Player.Position, targetMapCoord, _game.TargettingSpell.TargettingStyle);
                if (success)
                {
                    _game.SpellTargetSelected(target);
                    EndTargettingMode();
                }

                return true;
            }

            foreach (var key in TurnBasedGame.MovementDirectionMapping.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    var targetMapCoord = _targettedConsolePos + MapRenderer.ViewPort.Location;
                    if (!Map.FOV.CurrentFOV.Contains(targetMapCoord))
                    {
                        _targettedConsolePos = Player.Position;
                    }

                    var potentialTarget = _targettedConsolePos + TurnBasedGame.MovementDirectionMapping[key];
                    targetMapCoord = potentialTarget + MapRenderer.ViewPort.Location;
                    if (Map.FOV.CurrentFOV.Contains(targetMapCoord))
                    {
                        _targettedConsolePos = potentialTarget;
                    }
                    
                    return true;
                }
            }

            return base.ProcessKeyboard(info);
        }

        private bool PlayerTurnProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.F)
                && _appSettings.Debug)
            {
                ToggleFov();
                return true;
            }

            if (HandleGuiKeys(info))
            {
                return true;
            }

            if (_game.HandleAsPlayerInput(info))
            {
                _lastSummaryConsolePosition = default;
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        private bool InteractTargettingProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Escape))
            {
                EndTargettingMode();
                return true;
            }

            if (_game.HandleAsInteractTargettingInput(info))
            {
                return true;
            }

            if (HandleGuiKeys(info))
            {
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        public void BeginTargetting(SpellTemplate spell)
        {
            _game.StartSpellTargetting(spell);
            FlavorMessageChanged?.Invoke(this, $"Aiming {spell.Name}...");
        }

        private void EndTargettingMode()
        {
            _game.State = State.PlayerTurn;
            _game.TargetInteractables.Clear();
            FlavorMessageChanged?.Invoke(this, string.Empty);
        }

        private void ToggleFov()
        {
            if (Map.FovVisibilityHandler.Enabled)
            {
                Map.FovVisibilityHandler.Disable();
            }
            else
            {
                Map.FovVisibilityHandler.Enable();
            }
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            MapRenderer.CenterViewPortOnPoint(Player.Position);
        }

        private void Player_RemovedFromMap(object sender, System.EventArgs e)
        {
            _menuProvider.Death.Show("You died.");
        }
    }
}

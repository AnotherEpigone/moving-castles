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
        private readonly MouseHighlightConsole _mouseHighlight;
        private readonly InteractTargettingConsole _interactTargettingConsole;
        private readonly ITurnBasedGame _game;

        private Point _lastSummaryConsolePosition;

        public event System.EventHandler<ConsoleListEventArgs> SummaryConsolesChanged;
        public event System.EventHandler<string> FlavorMessageChanged;

        public DungeonMap Map { get; }

        public ScrollingConsole MapRenderer { get; }

        public Wizard Player { get; }

        public DungeonMapConsole(
            int viewportWidth,
            int viewportHeight,
            Font tilesetFont,
            IMapModeMenuProvider menuProvider,
            ITurnBasedGame game,
            DungeonMap map)
        {
            _menuProvider = menuProvider;
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
            IsFocused = true;

            Map.CalculateFOV(Player.Position, Player.FovRadius, Radius.SQUARE);
            MapRenderer.CenterViewPortOnPoint(Player.Position);

            Children.Add(MapRenderer);
            Children.Add(_mouseHighlight);
            Children.Add(_interactTargettingConsole);
        }

        private void Player_RemovedFromMap(object sender, System.EventArgs e)
        {
            _menuProvider.Death.Show("You died.");
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

            var mapCoord = new Coord(
                mapState.ConsoleCellPosition.X + MapRenderer.ViewPort.X,
                mapState.ConsoleCellPosition.Y + MapRenderer.ViewPort.Y);

            var coordIsTargetable = mapState.IsOnConsole && Map.FOV.CurrentFOV.Contains(mapCoord);
            _mouseHighlight.IsVisible = mapState.IsOnConsole && Map.Explored[mapCoord];
            _mouseHighlight.Draw(mapState, MapRenderer.ViewPort.Location, coordIsTargetable);

            if (coordIsTargetable && _lastSummaryConsolePosition != mapState.ConsoleCellPosition)
            {
                // update summaries
                var summaryControls = new List<Console>();
                foreach (var entity in Map.GetEntities<BasicEntity>(mapCoord))
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
            
            if (!_mouseHighlight.IsVisible && _lastSummaryConsolePosition != default)
            {
                // remove the summaries if we just moved out of a valid location
                _lastSummaryConsolePosition = default;
                SummaryConsolesChanged?.Invoke(this, new ConsoleListEventArgs(new List<Console>()));
            }

            if (coordIsTargetable && _game.State == State.Targetting)
            {
                TargettingProcessMouse(state, mapCoord);
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

            if (info.IsKeyPressed(Keys.Q))
            {
                EndTargettingMode();
                _menuProvider.SpellSelect.Show(
                    Player.GetGoRogueComponent<ISpellCastingComponent>().Spells,
                    selectedSpell => BeginTargetting(selectedSpell));

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

        private bool PlayerTurnProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Escape))
            {
                _menuProvider.Pop.Show();
                return true;
            }

            if (info.IsKeyPressed(Keys.F))
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

        private void BeginTargetting(SpellTemplate spell)
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
    }
}

using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.Components;
using MovingCastles.Components.Stats;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using MovingCastles.Serialization.Settings;
using SadConsole;
using SadConsole.Input;
using System;
using System.Linq;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace MovingCastles.Ui.Consoles
{
    internal class DungeonMapConsole : ContainerConsole, ITurnBasedGameConsole
    {
        private readonly IMapModeMenuProvider _menuProvider;
        private readonly IAppSettings _appSettings;
        private readonly ITurnBasedGame _game;
        private readonly int _mapRendererPadding;
        private readonly SadConsole.Console _mapRendererContainer;
        private Point _lastSummaryConsolePosition;
        private MouseHighlightConsole _mouseHighlight;
        private InteractTargettingConsole _interactTargettingConsole;

        public event EventHandler<ConsoleListEventArgs> SummaryConsolesChanged;
        public event EventHandler<string> FlavorMessageChanged;

        public Point _lastMousePos;
        public Point _targettedConsolePos;
        public string _statusMessage;

        public McMap Map { get; private set; }

        public ScrollingConsole MapRenderer { get; private set; }

        public Wizard Player { get; private set; }

        public SadConsole.Console ThisConsole => this;

        public DungeonMapConsole(
            int viewportWidth,
            int viewportHeight,
            Font tilesetFont,
            IMapModeMenuProvider menuProvider,
            ITurnBasedGame game,
            IAppSettings appSettings,
            McMap map)
        {
            IsFocused = true;

            _menuProvider = menuProvider;
            _appSettings = appSettings;
            _game = game;

            _mouseHighlight = new MouseHighlightConsole(viewportWidth, viewportHeight, tilesetFont, game, map);
            _interactTargettingConsole = new InteractTargettingConsole(tilesetFont, map);

            Map = map;
            _game.Init(map);

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

            Map.CalculateFOV(Player.Position, Player.FovRadius, Radius.SQUARE);

            _mapRendererPadding = (Math.Max(viewportWidth, viewportHeight) / 2) + 1;
            _mapRendererContainer = new SadConsole.Console(viewportWidth, viewportHeight, tilesetFont)
            {
                UseMouse = false,
            };
            SetMapRendererPosition(new Point(_mapRendererPadding, _mapRendererPadding));
            _mapRendererContainer.Children.Add(MapRenderer);

            MapRenderer.Children.Add(_mouseHighlight);
            MapRenderer.Children.Add(_interactTargettingConsole);

            CenterMapViewOnPlayer();

            Children.Add(_mapRendererContainer);
        }

        public void UnsetMap()
        {
            foreach (var entity in Map.Entities.Items.OfType<McEntity>())
            {
                if (entity is Wizard player)
                {
                    player.RemovedFromMap -= Player_RemovedFromMap;
                    Player.Moved -= Player_Moved;
                }

                _game.UnregisterEntity(entity);
            }

            Map.RemoveEntity(Player);
            Map = null;
        }

        public void SetMap(McMap map)
        {
            _mouseHighlight = new MouseHighlightConsole(_mouseHighlight.Width, _mouseHighlight.Height, _mouseHighlight.Font, _game, map);
            _interactTargettingConsole = new InteractTargettingConsole(_interactTargettingConsole.Font, map);

            Map = map;
            _game.Init(map);

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

            _mapRendererContainer.Children.Remove(MapRenderer);
            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, MapRenderer.Width, MapRenderer.Height), MapRenderer.Font);
            MapRenderer.UseMouse = false;
            Map.CalculateFOV(Player.Position, Player.FovRadius, Radius.SQUARE);

            SetMapRendererPosition(new Point(_mapRendererPadding, _mapRendererPadding));
            _mapRendererContainer.Children.Add(MapRenderer);

            MapRenderer.Children.Add(_mouseHighlight);
            MapRenderer.Children.Add(_interactTargettingConsole);

            CenterMapViewOnPlayer();

            Children.Add(_mapRendererContainer);
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
                var flavorDescriptions = Map
                    .GetEntities<McEntity>(mouseMapCoord)
                    .Select(entity => entity.GetFlavorDescription())
                    .Where(desc => !string.IsNullOrEmpty(desc))
                    .ToList();
                _lastSummaryConsolePosition = mapState.ConsoleCellPosition;
                FlavorMessageChanged.Invoke(this, string.Join(' ', new[] { _statusMessage }.Concat(flavorDescriptions)));
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

            _mouseHighlight.IsVisible = Map.FOV.CurrentFOV.Contains(_targettedConsolePos) && Map.Explored[targetMapCoord];
            _mouseHighlight.Draw(_targettedConsolePos, MapRenderer.ViewPort.Location, Map.FOV.CurrentFOV.Contains(targetMapCoord));

            if (!_mouseHighlight.IsVisible && _lastSummaryConsolePosition != default)
            {
                // just moved out of a valid location
                _lastSummaryConsolePosition = default;
                // TODO this logic isn't valid - triggered every tick outside of visible highlight
                ////FlavorMessageChanged.Invoke(this, string.Empty);
            }

            if (coordIsTargetable && _game.State == State.Targetting)
            {
                TargettingProcessMouse(state, mouseMapCoord);
            }

            return base.ProcessMouse(state);
        }

        public override void Update(TimeSpan timeElapsed)
        {
            _interactTargettingConsole.IsVisible = _game.State == State.InteractTargetting;
            _interactTargettingConsole.Draw(Player.Position - MapRenderer.ViewPort.Location, _game.TargetInteractables);

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
                    selectedSpell => OnSelectSpell(selectedSpell),
                    Player.GetGoRogueComponent<IEndowmentPoolComponent>().Value);

                return true;
            }

            if (info.IsKeyPressed(Keys.J))
            {
                EndTargettingMode();
                _menuProvider.Journal.Show(Player.JournalEntries);
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
                EndTargettingMode();
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

            if (info.IsKeyPressed(Keys.OemPeriod)
                && _game.TargetableTiles.Count > 0)
            {
                var targetMapCoord = _targettedConsolePos + MapRenderer.ViewPort.Location;
                var targetedTileIndex = _game.TargetableTiles.IndexOf(targetMapCoord);
                if (targetedTileIndex == -1 || targetedTileIndex == _game.TargetableTiles.Count - 1)
                {
                    _targettedConsolePos = _game.TargetableTiles[0] - MapRenderer.ViewPort.Location;
                }
                else
                {
                    _targettedConsolePos = _game.TargetableTiles[targetedTileIndex + 1] - MapRenderer.ViewPort.Location;
                }

                return true;
            }

            if (info.IsKeyPressed(Keys.OemComma)
                && _game.TargetableTiles.Count > 0)
            {
                var targetMapCoord = _targettedConsolePos + MapRenderer.ViewPort.Location;
                var targetedTileIndex = _game.TargetableTiles.IndexOf(targetMapCoord);
                if (targetedTileIndex == -1)
                {
                    _targettedConsolePos = _game.TargetableTiles[0] - MapRenderer.ViewPort.Location;
                }
                else if (targetedTileIndex == 0)
                {
                    _targettedConsolePos = _game.TargetableTiles.Last() - MapRenderer.ViewPort.Location;
                }
                else
                {
                    _targettedConsolePos = _game.TargetableTiles[targetedTileIndex - 1] - MapRenderer.ViewPort.Location;
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

        public void OnSelectSpell(SpellTemplate spell)
        {
            if (spell.TargettingStyle.TargetMode == TargetMode.Self)
            {
                _game.StartSpellTargetting(spell);
                _game.SpellTargetSelected(Player.Position);
            }
            else
            {
                StartTargetting(spell);
            }
        }

        public void StartTargetting(SpellTemplate spell)
        {
            _game.StartSpellTargetting(spell);

            _statusMessage = ColorHelper.GetParserString($"Aiming {spell.Name}.", ColorHelper.BayeuxRed);
            FlavorMessageChanged?.Invoke(this, _statusMessage);
            if (_game.TargetableTiles.Count > 0)
            {
                _targettedConsolePos = _game.TargetableTiles[0] - MapRenderer.ViewPort.Location;
            }
        }

        private void EndTargettingMode()
        {
            _game.State = State.PlayerTurn;
            _game.TargetInteractables.Clear();

            _statusMessage = string.Empty;
            FlavorMessageChanged?.Invoke(this, _statusMessage);
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

        private void CenterMapViewOnPlayer()
        {
            MapRenderer.CenterViewPortOnPoint(Player.Position);
            var rendererError = MapRenderer.ViewPort.Location - Player.Position;
            var target = new Point(_mapRendererContainer.Width / 2, _mapRendererContainer.Height / 2) + rendererError;
            SetMapRendererPosition(target);
        }

        private void SetMapRendererPosition(Point pos)
        {
            MapRenderer.Position = pos;
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            CenterMapViewOnPlayer();
        }

        private void Player_RemovedFromMap(object sender, System.EventArgs e)
        {
            _menuProvider.Death.Show("You died.");
        }
    }
}

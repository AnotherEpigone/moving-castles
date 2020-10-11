using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems.TurnBasedGame;
using MovingCastles.Maps;
using MovingCastles.Ui;
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
        private readonly Console _mouseHighlight;
        private readonly ITurnBasedGame _game;

        private Point _lastSummaryConsolePosition;
        public event System.EventHandler<ConsoleListEventArgs> SummaryConsolesChanged;

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

            _mouseHighlight = new Console(1, 1, tilesetFont);
            _mouseHighlight.SetGlyph(0, 0, 1, ColorHelper.WhiteHighlight);
            _mouseHighlight.UseMouse = false;

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

            Map.CalculateFOV(Player.Position, Player.FOVRadius, Radius.SQUARE);
            MapRenderer.CenterViewPortOnPoint(Player.Position);

            Children.Add(MapRenderer);
            Children.Add(_mouseHighlight);
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

            switch (_game.State)
            {
                case State.PlayerTurn:
                    return PlayerTurnProcessKeyboard(info);
                case State.Processing:
                    return base.ProcessKeyboard(info);
                case State.Targetting:
                    return TargettingProcessKeyboard(info);
                default:
                    return base.ProcessKeyboard(info);
            }
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

            _mouseHighlight.IsVisible = mapState.IsOnConsole && Map.Explored[mapCoord];
            _mouseHighlight.Position = mapState.ConsoleCellPosition;

            if (mapState.IsOnConsole
                && _lastSummaryConsolePosition != mapState.ConsoleCellPosition
                && Map.FOV.CurrentFOV.Contains(mapCoord))
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

            return base.ProcessMouse(state);
        }

        protected override void OnMouseLeftClicked(MouseConsoleState state)
        {
            if (_game.State != State.Targetting)
            {
                return;
            }
        }

        private bool PlayerTurnProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Escape))
            {
                _menuProvider.Pop.Show();
                return true;
            }

            if (info.IsKeyPressed(Keys.I))
            {
                _menuProvider.Inventory.Show(Player.GetGoRogueComponent<IInventoryComponent>());
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
                _game.State = State.PlayerTurn;
                _menuProvider.Pop.Show();
                return true;
            }

            if (info.IsKeyPressed(Keys.I))
            {
                _game.State = State.PlayerTurn;
                _menuProvider.Inventory.Show(Player.GetGoRogueComponent<IInventoryComponent>());
                return true;
            }

            // handle enter as confirm target

            return base.ProcessKeyboard(info);
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            MapRenderer.CenterViewPortOnPoint(Player.Position);
        }
    }
}

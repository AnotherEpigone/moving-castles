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

namespace MovingCastles.Consoles
{
    internal class MapConsole : ContainerConsole
    {
        private readonly IMenuProvider _menuProvider;
        private readonly Console _mouseHighlight;
        private readonly ITurnBasedGame _game;

        private Point _lastSummaryConsolePosition;

        public event System.EventHandler<ConsoleListEventArgs> SummaryConsolesChanged;

        public MovingCastlesMap Map { get; }

        public ScrollingConsole MapRenderer { get; }

        public Player Player { get; }

        public MapConsole(
            int viewportWidth,
            int viewportHeight,
            Font tilesetFont,
            IMenuProvider menuProvider,
            ITurnBasedGame game,
            MovingCastlesMap map)
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
                if (entity is Player player)
                {
                    Player = player;
                    _game.RegisterPlayer(player);
                    player.RemovedFromMap += Player_RemovedFromMap;
                    Player.Moved += Player_Moved;
                    continue;
                }

                _game.RegisterEntity(entity);
            }

            // Get a console that's set up to render the map, and add it as a child of this container so it renders
            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, viewportWidth, viewportHeight), tilesetFont);
            MapRenderer.UseMouse = false;
            IsFocused = true;

            // Calculate initial FOV and center camera
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
                SummaryConsolesChanged.Invoke(this, new ConsoleListEventArgs(summaryControls));
            }

            return base.ProcessMouse(state);
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            MapRenderer.CenterViewPortOnPoint(Player.Position);
        }
    }
}

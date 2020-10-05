using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.Components;
using MovingCastles.Entities;
using MovingCastles.Maps;
using SadConsole;
using SadConsole.Input;
using System.Collections.Generic;
using System.Linq;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace MovingCastles.Ui.Consoles
{
    public class CastleMapConsole : ContainerConsole
    {
        private readonly IMapModeMenuProvider _menuProvider;
        private readonly Console _mouseHighlight;

        private Point _lastSummaryConsolePosition;
        public event System.EventHandler<ConsoleListEventArgs> SummaryConsolesChanged;

        public CastleMap Map { get; }

        public ScrollingConsole MapRenderer { get; }

        public Castle Castle { get; }

        public CastleMapConsole(
            int width,
            int height,
            Font font,
            IMapModeMenuProvider menuProvider,
            CastleMap map)
        {
            _menuProvider = menuProvider;

            _mouseHighlight = new Console(1, 1, font);
            _mouseHighlight.SetGlyph(0, 0, 1, ColorHelper.WhiteHighlight);
            _mouseHighlight.UseMouse = false;

            Map = map;

            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, width, height), font);
            MapRenderer.UseMouse = false;

            IsFocused = true;

            Castle = map.Castle;

            Map.CalculateFOV(Castle.Position, Castle.FOVRadius, Radius.DIAMOND);
            MapRenderer.CenterViewPortOnPoint(Castle.Position);

            Children.Add(MapRenderer);
            Children.Add(_mouseHighlight);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Escape))
            {
                _menuProvider.Pop.Show();
                return true;
            }

            if (info.IsKeyPressed(Keys.I))
            {
                // todo - inventory from player/castle! fix this with proper Player class (not entity)
                //_menuProvider.Inventory.Show(Player.GetGoRogueComponent<IInventoryComponent>());
                // show empty inventory for now
                _menuProvider.Inventory.Show(new InventoryComponent());
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        public override bool ProcessMouse(MouseConsoleState state)
        {
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
    }
}

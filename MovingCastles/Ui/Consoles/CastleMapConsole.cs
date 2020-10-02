using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using SadConsole;
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
    }
}

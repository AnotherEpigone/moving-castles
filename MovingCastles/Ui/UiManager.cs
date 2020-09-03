using SadConsole;
using System;
using Console = SadConsole.Console;

namespace MovingCastles.Ui
{
    public sealed class UiManager
    {
        public const string TilesetFontPath = "Fonts\\kenney.font";
        public const string TilesetFontName = "kenney";

        private readonly Lazy<ContainerConsole> _screen;

        public UiManager()
        {
            _screen = new Lazy<ContainerConsole>(InitScreen);
        }

        public int ViewPortWidth { get; } = 80;
        public int ViewPortHeight { get; } = 25;
        public Console MapConsole { get; private set; }
        public Console EventLogConsole { get; private set; }

        public ContainerConsole Screen => _screen.Value;

        private ContainerConsole InitScreen()
        {
            MapConsole = new MapScreen(40, 25, ViewPortWidth, ViewPortHeight);

            var screen = new ContainerConsole();
            screen.Children.Add(MapConsole);

            return screen;
        }
    }
}

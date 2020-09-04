using MovingCastles.Maps;
using SadConsole;
using System;
using Console = SadConsole.Console;

namespace MovingCastles.Ui
{
    public sealed class UiManager
    {
        public const string TilesetFontPath = "Fonts\\sprites.font";
        public const string TilesetFontName = "sprites";

        private readonly Lazy<ContainerConsole> _screen;

        public UiManager()
        {
            _screen = new Lazy<ContainerConsole>(InitScreen);
        }

        public Font GuiTextFont { get; set; }
        public int ViewPortWidth { get; } = 80;
        public int ViewPortHeight { get; } = 25;
        public Console MapConsole { get; private set; }
        public MessageLog EventLog { get; private set; }

        public ContainerConsole Screen => _screen.Value;

        private ContainerConsole InitScreen()
        {
            var mapConsole = new MapScreen(40, 25, ViewPortWidth, (int)Math.Round(ViewPortHeight * 0.9));

            EventLog = new MessageLog(ViewPortWidth, ViewPortHeight - mapConsole.Height, GuiTextFont);
            EventLog.Position = new Microsoft.Xna.Framework.Point(0, mapConsole.MapRenderer.ViewPort.Height);
            EventLog.Add("test message1");
            EventLog.Add("test message2");
            EventLog.Add("test message3");

            MapConsole = mapConsole;

            var screen = new ContainerConsole();
            screen.Children.Add(mapConsole);
            screen.Children.Add(EventLog);

            return screen;
        }
    }
}

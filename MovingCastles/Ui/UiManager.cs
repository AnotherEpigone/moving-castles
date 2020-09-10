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

        public int ViewPortWidth { get; } = 160; // 160 x 8 = 1280
        public int ViewPortHeight { get; } = 45; // 45 x 16 = 720

        public Console CreateMainMenu()
        {
            return new MainMenu(this);
        }

        public ContainerConsole CreateMapScreen()
        {
            var tilesetFont = Global.Fonts[TilesetFontName].GetFont(Font.FontSizes.One);
            var mapConsole = new MapScreen(80, 45, ViewPortWidth, (int)Math.Round(ViewPortHeight * 0.9), tilesetFont);

            var eventLog = new MessageLog(ViewPortWidth, ViewPortHeight - mapConsole.Height, Global.FontDefault);
            eventLog.Position = new Microsoft.Xna.Framework.Point(0, mapConsole.MapRenderer.ViewPort.Height);
            eventLog.Add("test message1");
            eventLog.Add("test message2");
            eventLog.Add("test message3");

            var screen = new ContainerConsole();
            screen.Children.Add(mapConsole);
            screen.Children.Add(eventLog);

            return screen;
        }
    }
}

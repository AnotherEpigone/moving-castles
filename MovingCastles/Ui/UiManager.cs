using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.GameSystems.Items;
using MovingCastles.Maps;
using SadConsole;
using SadConsole.Controls;
using System.Linq;
using Console = SadConsole.Console;

namespace MovingCastles.Ui
{
    public sealed class UiManager
    {
        public const string TilesetFontPath = "Fonts\\sprites.font";
        public const string TilesetFontName = "sprites";

        public static Color MidnightestBlue = new Color(3, 3, 15);
        public static Color MidnighterBlue = new Color(5, 5, 25);

        public int ViewPortWidth { get; } = 160; // 160 x 8 = 1280
        public int ViewPortHeight { get; } = 45; // 45 x 16 = 720

        public Console CreateMainMenu()
        {
            return new MainMenu(this);
        }

        private IMenuProvider CreateMenuProvider()
        {
            var inventory = new InventoryWindow(120, 30);

            return new MenuProvider(inventory);
        }

        public ContainerConsole CreateMapScreen()
        {
            const int leftPaneWidth = 30;
            const int topPaneHeight = 3;
            const int eventLogHeight = 4;

            var leftPane = new Console(leftPaneWidth, ViewPortHeight);
            leftPane.Fill(null, MidnightestBlue, null);

            var rightSectionWidth = ViewPortWidth - leftPaneWidth;

            var topPane = new Console(rightSectionWidth, topPaneHeight);
            topPane.Position = new Point(leftPaneWidth, 0);
            topPane.Fill(null, MidnightestBlue, null);

            var tilesetFont = Global.Fonts[TilesetFontName].GetFont(Font.FontSizes.One);
            var tileSizeXFactor = tilesetFont.Size.X / Global.FontDefault.Size.X;
            var mapConsole = new MapScreen(
                80,
                45,
                rightSectionWidth/tileSizeXFactor,
                ViewPortHeight - eventLogHeight - topPaneHeight,
                tilesetFont,
                CreateMenuProvider());
            mapConsole.Position = new Point(leftPaneWidth, topPaneHeight);


            var eventLog = new MessageLog(ViewPortWidth, eventLogHeight, Global.FontDefault);
            eventLog.Position = new Point(leftPaneWidth, mapConsole.MapRenderer.ViewPort.Height + topPaneHeight);


            // test data...
            mapConsole.Player.GetGoRogueComponent<IInventoryComponent>().Items.Add(new InventoryItem("trusty oak staff"));
            eventLog.Add("Hello world.");
            leftPane.Print(3, 1, "Test dungeon");

            var screen = new ContainerConsole();
            screen.Children.Add(leftPane);
            screen.Children.Add(topPane);
            screen.Children.Add(mapConsole);
            screen.Children.Add(eventLog);

            return screen;
        }
    }
}

using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.GameSystems.Items;
using MovingCastles.Maps;
using SadConsole;
using SadConsole.Controls;
using Console = SadConsole.Console;

namespace MovingCastles.Ui
{
    public sealed class UiManager
    {
        public const string TilesetFontPath = "Fonts\\sprites.font";
        public const string TilesetFontName = "sprites";

        public static Color MidnightestBlue = new Color(3, 3, 15);
        public static Color MidnighterBlue = new Color(5, 5, 25);
        public static Color ManaBlue = new Color(15, 45, 85);

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


            var leftPane = new ControlsConsole(leftPaneWidth, ViewPortHeight);
            var manaBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 4),
            };
            manaBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(Color.White, ManaBlue);
            manaBar.Progress = 1;
            leftPane.Add(manaBar);

            var healthBar = new ProgressBar(30, 1, HorizontalAlignment.Left)
            {
                Position = new Point(0, 3),
            };
            healthBar.ThemeColors = ColorHelper.GetProgressBarThemeColors(Color.White, Color.Red);
            healthBar.Progress = 1;
            leftPane.Add(healthBar);
            leftPane.Add(new Label("Vede of Tattersail") { Position = new Point(1, 0), TextColor = Color.White });
            leftPane.Add(new Label("Material Plane, Ayen") { Position = new Point(1, 1), TextColor = Color.DarkGray });
            leftPane.Add(new Label("Old Alward's Tower") { Position = new Point(1, 2), TextColor = Color.DarkGray });

            var rightSectionWidth = ViewPortWidth - leftPaneWidth;

            var topPane = new Console(rightSectionWidth, topPaneHeight);
            topPane.Position = new Point(leftPaneWidth, 0);
            topPane.Fill(null, MidnightestBlue, null);

            var tilesetFont = Global.Fonts[TilesetFontName].GetFont(Font.FontSizes.One);
            var tileSizeXFactor = tilesetFont.Size.X / Global.FontDefault.Size.X;
            var mapConsole = new MapConsole(
                80,
                45,
                rightSectionWidth / tileSizeXFactor,
                ViewPortHeight - eventLogHeight - topPaneHeight,
                tilesetFont,
                CreateMenuProvider())
            {
                Position = new Point(leftPaneWidth, topPaneHeight)
            };

            var eventLog = new MessageLog(ViewPortWidth, eventLogHeight, Global.FontDefault);
            eventLog.Position = new Point(leftPaneWidth, mapConsole.MapRenderer.ViewPort.Height + topPaneHeight);

            // test data...
            mapConsole.Player.GetGoRogueComponent<IInventoryComponent>().Items.Add(new InventoryItem(
                "trusty oak staff",
                "Cut from the woods of the Academy at Kurisau, this staff has served you since you first learned to sense the Wellspring."));
            eventLog.Add("Hello world.");

            var screen = new ContainerConsole();
            screen.Children.Add(leftPane);
            screen.Children.Add(topPane);
            screen.Children.Add(mapConsole);
            screen.Children.Add(eventLog);

            return screen;
        }
    }
}

using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using SadConsole;
using Console = SadConsole.Console;

namespace MovingCastles.Ui
{
    public sealed class UiManager
    {
        private readonly ILogManager _logManager;

        public const string TilesetFontPath = "Fonts\\sprites.font";
        public const string TilesetFontName = "sprites";

        public static Color MidnightestBlue = new Color(3, 3, 15);
        public static Color MidnighterBlue = new Color(5, 5, 25);
        public static Color ManaBlue = new Color(15, 45, 85);

        public int ViewPortWidth { get; } = 160; // 160 x 8 = 1280
        public int ViewPortHeight { get; } = 45; // 45 x 16 = 720

        public UiManager()
        {
            _logManager = new LogManager();
        }

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
            var tilesetFont = Global.Fonts[TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            return new MapScreen(
                ViewPortWidth,
                ViewPortHeight,
                tilesetFont,
                CreateMenuProvider(),
                entityFactory,
                _logManager);
        }
    }
}

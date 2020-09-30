using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using SadConsole;
using Console = SadConsole.Console;

namespace MovingCastles.Ui
{
    public sealed class UiManager : IUiManager
    {
        private readonly ILogManager _logManager;

        public const string TilesetFontPath = "Fonts\\sprites.font";
        public const string TilesetFontName = "sprites";

        public int ViewPortWidth { get; } = 160; // 160 x 8 = 1280
        public int ViewPortHeight { get; } = 45; // 45 x 16 = 720

        public UiManager()
        {
            _logManager = new LogManager();
        }

        public ContainerConsole CreateMapScreen(IMapPlan mapPlan)
        {
            var tilesetFont = Global.Fonts[TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);
            return new MapScreen(
                ViewPortWidth,
                ViewPortHeight,
                tilesetFont,
                CreateMenuProvider(),
                mapFactory,
                mapPlan,
                _logManager);
        }

        private IMenuProvider CreateMenuProvider()
        {
            var inventory = new InventoryWindow(120, 30);

            return new MenuProvider(inventory);
        }
    }
}

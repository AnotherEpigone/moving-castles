using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using SadConsole;
using Console = SadConsole.Console;

namespace MovingCastles.Ui
{
    public sealed class UiManager : IUiManager
    {
        private readonly ILogManager _logManager;
        private readonly IGameManager _gameManager;

        public const string TilesetFontPath = "Fonts\\sprites.font";
        public const string TilesetFontName = "sprites";

        public int ViewPortWidth { get; } = 160; // 160 x 8 = 1280
        public int ViewPortHeight { get; } = 45; // 45 x 16 = 720

        public UiManager()
        {
            _logManager = new LogManager();
        }

        public void ShowMainMenu(IGameManager gameManager)
        {
            var menu = new MainMenu(gameManager);
            Global.CurrentScreen = menu;
            menu.IsFocused = true;
        }

        public ContainerConsole CreateMapScreen(IMapPlan mapPlan, IGameManager gameManager)
        {
            var tilesetFont = Global.Fonts[TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);
            return new MapScreen(
                ViewPortWidth,
                ViewPortHeight,
                tilesetFont,
                CreateMenuProvider(gameManager),
                mapFactory,
                mapPlan,
                _logManager);
        }

        private IMenuProvider CreateMenuProvider(IGameManager gameManager)
        {
            var inventory = new InventoryWindow(120, 30);
            var death = new DeathWindow(this, gameManager);

            return new MenuProvider(inventory, death);
        }
    }
}

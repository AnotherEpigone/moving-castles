using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Player;
using MovingCastles.Maps;
using MovingCastles.Ui.Consoles;
using MovingCastles.Ui.Windows;
using SadConsole;

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

        public void ShowMainMenu(IGameManager gameManager)
        {
            var menu = new MainMenuConsole(gameManager);
            Global.CurrentScreen = menu;
            menu.IsFocused = true;
        }

        public ContainerConsole CreateDungeonMapScreen(IMapPlan mapPlan, IGameManager gameManager)
        {
            var tilesetFont = Global.Fonts[TilesetFontName].GetFont(Font.FontSizes.One);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);
            return new DungeonModeConsole(
                ViewPortWidth,
                ViewPortHeight,
                tilesetFont,
                CreateMenuProvider(gameManager),
                mapFactory,
                mapPlan,
                _logManager,
                Player.CreateDefault());
        }

        public CastleModeConsole CreateCastleMapScreen(IMapPlan mapPlan, IGameManager gameManager)
        {
            var tilesetFont = Global.Fonts[TilesetFontName].GetFont(Font.FontSizes.Three);
            var entityFactory = new EntityFactory(tilesetFont, _logManager);
            var mapFactory = new MapFactory(entityFactory);
            return new CastleModeConsole(
                ViewPortWidth,
                ViewPortHeight,
                tilesetFont,
                CreateMenuProvider(gameManager),
                mapFactory,
                mapPlan,
                _logManager,
                Player.CreateDefault());
        }

        private IMapModeMenuProvider CreateMenuProvider(IGameManager gameManager)
        {
            var inventory = new InventoryWindow(120, 30);
            var death = new DeathWindow(this, gameManager);
            var pop = new PopupMenuWindow(this, gameManager);

            return new MapModeMenuProvider(inventory, death, pop);
        }
    }
}

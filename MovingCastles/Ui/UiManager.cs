using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using MovingCastles.Serialization.Settings;
using MovingCastles.Ui.Consoles;
using MovingCastles.Ui.Windows;
using SadConsole;

namespace MovingCastles.Ui
{
    public sealed class UiManager : IUiManager
    {
        private readonly ILogManager _logManager;
        private readonly IAppSettings _appSettings;

        public const string DungeonFontPath = "Fonts\\dungeon.font";
        public const string DungeonFontName = "dungeon";

        public const string CastleFontPath = "Fonts\\castle.font";
        public const string CastleFontName = "castle";

        public int ViewPortWidth { get; private set; } = 160; // 160 x 8 = 1280
        public int ViewPortHeight { get; private set; } = 45; // 45 x 16 = 720

        public UiManager(ILogManager logManager, IAppSettings appSettings)
        {
            _logManager = logManager;
            _appSettings = appSettings;
        }

        public void SetViewport(int width, int height)
        {
            Settings.ResizeWindow(width, height);

            ViewPortWidth = width / Global.CurrentScreen.Font.Size.X;
            ViewPortHeight = height / Global.CurrentScreen.Font.Size.Y;
            Global.CurrentScreen.Resize(ViewPortWidth, ViewPortHeight, false);
        }

        public void ToggleFullScreen()
        {
            Settings.ToggleFullScreen();

            ViewPortWidth = Global.WindowWidth / Global.CurrentScreen.Font.Size.X;
            ViewPortHeight = Global.WindowHeight / Global.CurrentScreen.Font.Size.Y;
            Global.CurrentScreen.Resize(ViewPortWidth, ViewPortHeight, false);
        }

        public void ShowMainMenu(IGameManager gameManager)
        {
            var menu = new MainMenuConsole(this, gameManager, _appSettings, ViewPortWidth, ViewPortHeight);
            Global.CurrentScreen = menu;
            menu.IsFocused = true;
        }

        public MainConsole CreateMapScreen(
            IGameManager gameManager,
            ITurnBasedGame game,
            ITurnBasedGameConsoleFactory turnBasedGameConsoleFactory,
            IDungeonMaster dungeonMaster,
            Font tilesetFont)
        {
            return new MainConsole(
                ViewPortWidth,
                ViewPortHeight,
                tilesetFont,
                CreateMenuProvider(gameManager, dungeonMaster),
                dungeonMaster,
                _logManager,
                game,
                _appSettings,
                turnBasedGameConsoleFactory);
        }

        private IMapModeMenuProvider CreateMenuProvider(IGameManager gameManager, IDungeonMaster dungeonMaster)
        {
            var inventory = new InventoryWindow(120, 30, dungeonMaster);
            var death = new DeathWindow(this, gameManager);
            var pop = new PopupMenuWindow(this, gameManager);
            var spellSelect = new SpellSelectionWindow();
            var commands = new CommandWindow();
            var journal = new JournalWindow(120, 30);

            return new MapModeMenuProvider(inventory, death, pop, spellSelect, commands, journal);
        }
    }
}

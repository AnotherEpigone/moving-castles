using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.TurnBased;
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

        public int ViewPortWidth { get; private set; } = 160; // 160 x 8 = 1280
        public int ViewPortHeight { get; private set; } = 45; // 45 x 16 = 720

        public UiManager(ILogManager logManager)
        {
            _logManager = logManager;
        }

        public void ActivateFullScreen()
        {
            Settings.ToggleFullScreen();

            ViewPortWidth = Global.WindowWidth / Global.CurrentScreen.Font.Size.X;
            ViewPortHeight = Global.WindowHeight / Global.CurrentScreen.Font.Size.Y;
            Global.CurrentScreen.Resize(ViewPortWidth, ViewPortHeight, false);
        }

        public void ShowMainMenu(IGameManager gameManager)
        {
            var menu = new MainMenuConsole(this, gameManager, ViewPortWidth, ViewPortHeight);
            Global.CurrentScreen = menu;
            menu.IsFocused = true;
        }

        public DungeonModeConsole CreateDungeonMapScreen(
            IGameManager gameManager,
            ITurnBasedGame turnBasedGame,
            DungeonMap map,
            Font tilesetFont)
        {
            return new DungeonModeConsole(
                ViewPortWidth,
                ViewPortHeight,
                tilesetFont,
                CreateMenuProvider(gameManager),
                map,
                _logManager,
                turnBasedGame);
        }

        public CastleModeConsole CreateCastleMapScreen(
            IGameManager gameManager,
            CastleMap map,
            Font tilesetFont)
        {
            return new CastleModeConsole(
                ViewPortWidth,
                ViewPortHeight,
                tilesetFont,
                CreateMenuProvider(gameManager),
                map,
                _logManager);
        }

        private IMapModeMenuProvider CreateMenuProvider(IGameManager gameManager)
        {
            var inventory = new InventoryWindow(120, 30);
            var death = new DeathWindow(this, gameManager);
            var pop = new PopupMenuWindow(this, gameManager);
            var spellSelect = new SpellSelectionWindow();
            var commands = new CommandWindow();

            return new MapModeMenuProvider(inventory, death, pop, spellSelect, commands);
        }
    }
}

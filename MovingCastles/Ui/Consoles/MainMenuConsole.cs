using Microsoft.Xna.Framework;
using MovingCastles.GameSystems;
using MovingCastles.Serialization.Settings;
using MovingCastles.Ui.Controls;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;

namespace MovingCastles.Ui.Consoles
{
    public sealed class MainMenuConsole : ContainerConsole
    {
        private readonly McControlsConsole _menuConsole;
        private readonly McControlsConsole _settingsConsole;

        private McControlsConsole _activeLowerConsole;

        public MainMenuConsole(IUiManager uiManager, IGameManager gameManager, IAppSettings appSettings, int width, int height)
        {
            DefaultBackground = Color.Black;

            var titleFont = Global.FontDefault.Master.GetFont(Font.FontSizes.Three);
            var titleConsole = new Console(width, 12, titleFont);

            titleConsole.Fill(null, ColorHelper.MidnightestBlue, null);
            titleConsole.Print((width / 6) - 7, 3, "MOVING CASTLES");
            titleConsole.DefaultBackground = ColorHelper.MidnightestBlue;

            _menuConsole = CreateMenuConsole(gameManager, width, height - titleConsole.Height);
            _menuConsole.Position = new Point(0, titleConsole.Height);

            _settingsConsole = CreateSettingsConsole(uiManager, gameManager, appSettings, width, height - titleConsole.Height);
            _settingsConsole.Position = new Point(0, titleConsole.Height);
            _settingsConsole.IsVisible = false;

            Children.Add(titleConsole);
            Children.Add(_menuConsole);
            Children.Add(_settingsConsole);

            FocusConsole(_menuConsole);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            return _menuConsole.ProcessKeyboard(info);
        }

        private void FocusConsole(McControlsConsole toFocus)
        {
            if (_activeLowerConsole != null)
            {
                _activeLowerConsole.IsVisible = false;
                _activeLowerConsole.IsFocused = false;
            }

            toFocus.IsVisible = true;
            toFocus.IsFocused = true;
            _activeLowerConsole = toFocus;
        }

        private McControlsConsole CreateSettingsConsole(IUiManager uiManager, IGameManager gameManager, IAppSettings appSettings, int width, int height)
        {
            var settingsConsole = new McControlsConsole(width, height);

            var buttonX = (width / 2) - 12;
            const int topButtonY = 8;

            var fullscreenToggleButton = new McSelectionButton(26, 1)
            {
                Text = "Toggle fullscreen",
                Position = new Point(buttonX, topButtonY),
            };
            fullscreenToggleButton.Click += (_, __) =>
            {
                appSettings.FullScreen = !appSettings.FullScreen;
                uiManager.ToggleFullScreen();
                uiManager.ShowMainMenu(gameManager);
            };

            var backButton = new McSelectionButton(26, 1)
            {
                Text = "Back",
                Position = new Point(buttonX, topButtonY + 2),
            };
            backButton.Click += (_, __) => FocusConsole(_menuConsole);

            settingsConsole.SetupSelectionButtons(fullscreenToggleButton, backButton);

            return settingsConsole;
        }

        private McControlsConsole CreateMenuConsole(IGameManager gameManager, int width, int height)
        {
            var menuConsole = new McControlsConsole(width, height);

            var buttonX = (width / 2) - 12;
            const int topButtonY = 8;
            var continueButton = new McSelectionButton(26, 1)
            {
                IsEnabled = gameManager.CanLoad(),
                Text = "Continue",
                Position = new Point(buttonX, topButtonY),
            };
            continueButton.Click += (_, __) => gameManager.Load();

            var newGameButton = new McSelectionButton(26, 1)
            {
                Text = "New Game",
                Position = new Point(buttonX, topButtonY + 2),
            };
            newGameButton.Click += (_, __) => gameManager.StartNewGame();

            var settingsButton = new McSelectionButton(26, 1)
            {
                Text = "Settings",
                Position = new Point(buttonX, topButtonY + 4),
            };
            settingsButton.Click += (_, __) => FocusConsole(_settingsConsole);

            var exitButton = new McSelectionButton(26, 1)
            {
                Text = "Exit",
                Position = new Point(buttonX, topButtonY + 6),
            };
            exitButton.Click += (_, __) => SadConsole.Game.Instance.Exit();

            var debugButtonX = (width / 2) - 14;
            var debugLabel = new Label("DEBUG")
            {
                TextColor = Color.White,
                Position = new Point(debugButtonX, topButtonY + 12),
            };

            var dungeonModeButton = new McSelectionButton(30, 1)
            {
                Text = "Dungeon mode testarea",
                Position = new Point(debugButtonX, topButtonY + 14),
            };
            dungeonModeButton.Click += (_, __) => gameManager.StartDungeonModeDemo();

            var castleModeButton = new McSelectionButton(30, 1)
            {
                Text = "Castle mode testarea",
                Position = new Point(debugButtonX, topButtonY + 16),
            };
            castleModeButton.Click += (_, __) => gameManager.StartCastleModeDemo();

            var mapTestButton = new McSelectionButton(30, 1)
            {
                Text = "Map generation testarea",
                Position = new Point(debugButtonX, topButtonY + 18),
            };
            mapTestButton.Click += (_, __) => gameManager.StartMapGenDemo();

            menuConsole.Add(debugLabel);
            menuConsole.SetupSelectionButtons(continueButton, newGameButton, settingsButton, exitButton, dungeonModeButton, castleModeButton, mapTestButton);

            return menuConsole;
        }
    }
}

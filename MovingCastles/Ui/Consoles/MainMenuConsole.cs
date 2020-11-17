using Microsoft.Xna.Framework;
using MovingCastles.GameSystems;
using MovingCastles.Ui.Controls;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;

namespace MovingCastles.Ui.Consoles
{
    public sealed class MainMenuConsole : ContainerConsole
    {
        private readonly McControlsConsole _menuConsole;

        public MainMenuConsole(IGameManager gameManager)
        {
            DefaultBackground = Color.Black;

            var titleFont = Global.FontDefault.Master.GetFont(Font.FontSizes.Three);
            var titleConsole = new Console(160, 45, titleFont);

            titleConsole.Fill(null, ColorHelper.MidnightestBlue, null);
            titleConsole.Print(20, 3, "MOVING CASTLES");
            titleConsole.DefaultBackground = ColorHelper.MidnightestBlue;

            _menuConsole = new McControlsConsole(160, 33)
            {
                Position = new Point(0, 12),
            };

            var continueButton = new McSelectionButton(30, 1)
            {
                IsEnabled = gameManager.CanLoad(),
                Text = "Continue",
                Position = new Point(67, 8),
            };
            continueButton.Click += (_, __) => gameManager.Load();

            var newGameButton = new McSelectionButton(30, 1)
            {
                Text = "New Game",
                Position = new Point(67, 10),
            };
            newGameButton.Click += (_, __) => gameManager.StartNewGame();

            var dungeonModeButton = new McSelectionButton(30, 1)
            {
                Text = "Dungeon mode testarea",
                Position = new Point(67, 12),
            };
            dungeonModeButton.Click += (_, __) => gameManager.StartDungeonModeDemo();

            var castleModeButton = new McSelectionButton(30, 1)
            {
                Text = "Castle mode testarea",
                Position = new Point(67, 14),
            };
            castleModeButton.Click += (_, __) => gameManager.StartCastleModeDemo();

            var mapTestButton = new McSelectionButton(30, 1)
            {
                Text = "Map generation testarea",
                Position = new Point(67, 16),
            };
            mapTestButton.Click += (_, __) => gameManager.StartMapGenDemo();

            var exitButton = new McSelectionButton(30, 1)
            {
                Text = "Exit",
                Position = new Point(67, 18),
            };
            exitButton.Click += (_, __) => SadConsole.Game.Instance.Exit();

            _menuConsole.SetupSelectionButtons(continueButton, newGameButton, dungeonModeButton, castleModeButton, mapTestButton, exitButton);
            _menuConsole.IsFocused = true;

            Children.Add(titleConsole);
            Children.Add(_menuConsole);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            return _menuConsole.ProcessKeyboard(info);
        }
    }
}

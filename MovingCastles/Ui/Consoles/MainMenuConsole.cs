using Microsoft.Xna.Framework;
using MovingCastles.GameSystems;
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

            var continueButton = new SelectionButton(26, 1)
            {
                Text = "Dungeon mode testarea",
                Position = new Point(67, 8),
            };
            continueButton.Click += (_, __) => gameManager.StartDungeonModeDemo();

            var newGameButton = new SelectionButton(26, 1)
            {
                Text = "Castle mode testarea",
                Position = new Point(67, 10),
            };

            newGameButton.Click += (_, __) => gameManager.StartCastleModeDemo();

            var exitButton = new SelectionButton(26, 1)
            {
                Text = "Exit",
                Position = new Point(67, 12),
            };
            exitButton.Click += (_, __) => SadConsole.Game.Instance.Exit();

            _menuConsole.SetupSelectionButtons(continueButton, newGameButton, exitButton);
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

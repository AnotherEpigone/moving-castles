using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.GameSystems;
using SadConsole;
using SadConsole.Controls;

namespace MovingCastles.Ui.Windows
{
    public class PopupMenuWindow : Window
    {
        private bool _escReleased = false;

        public PopupMenuWindow(IUiManager uiManager, IGameManager gameManager)
            : base(40, 4)
        {
            CloseOnEscKey = false; // it would close as soon as it opens...
            IsModalDefault = true;
            Center();

            var background = new Console(Width, Height);
            background.Fill(null, ColorHelper.GreyHighlight, null);

            Children.Add(background);

            var mainMenuText = "Exit to Main Menu";
            var mainMenuButtonWidth = mainMenuText.Length + 4;
            var mainMenuButton = new Button(mainMenuButtonWidth)
            {
                Text = mainMenuText,
                Position = new Point((Width / 2) - (mainMenuButtonWidth / 2), Height - 3),
            };
            mainMenuButton.Click += (_, __) =>
            {
                Hide();
                uiManager.ShowMainMenu(gameManager);
            };

            var quitText = "Exit to Desktop";
            var quitButtonWidth = mainMenuText.Length + 4;
            var quitButton = new Button(quitButtonWidth)
            {
                Text = quitText,
                Position = new Point((Width / 2) - (quitButtonWidth / 2), Height - 1),
            };
            quitButton.Click += (_, __) =>
            {
                System.Environment.Exit(0);
            };

            Add(mainMenuButton);
            Add(quitButton);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (!info.IsKeyPressed(Keys.Escape))
            {
                _escReleased = true;
                return base.ProcessKeyboard(info);
            }

            if (_escReleased)
            {
                Hide();
                return true;
            }

            return base.ProcessKeyboard(info);
        }
    }
}

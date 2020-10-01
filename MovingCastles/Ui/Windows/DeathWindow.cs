using Microsoft.Xna.Framework;
using MovingCastles.GameSystems;
using SadConsole;
using SadConsole.Controls;

namespace MovingCastles.Ui.Windows
{
    public class DeathWindow : Window
    {
        private readonly Label _deathLabel;

        public DeathWindow(IUiManager uiManager, IGameManager gameManager)
        : base(40, 3)
        {
            CloseOnEscKey = false;
            Center();

            var background = new Console(Width, Height);
            background.Fill(null, ColorHelper.GreyHighlight, null);

            Children.Add(background);

            _deathLabel = new Label(Width);
            var mainMenuButton = new Button(13)
            {
                Text = "Main Menu",
                Position = new Point(Width / 2, Height - 1),
            };
            mainMenuButton.Click += (_, __) =>
            {
                Hide();
                uiManager.ShowMainMenu(gameManager);
            };

            Add(_deathLabel);
            Add(mainMenuButton);
        }

        public void Show(string message)
        {
            _deathLabel.DisplayText = TextHelper.TruncateString(message, Width);
            base.Show(true);
        }
    }
}
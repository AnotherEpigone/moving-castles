using Microsoft.Xna.Framework;
using MovingCastles.GameSystems;
using SadConsole;

namespace MovingCastles.Ui.Consoles
{
    public sealed class MainMenuConsole : ContainerConsole
    {
        public MainMenuConsole(IGameManager gameManager)
        {
            DefaultBackground = Color.Black;

            var titleFont = Global.FontDefault.Master.GetFont(Font.FontSizes.Three);
            var titleConsole = new Console(160, 45, titleFont);

            titleConsole.Fill(null, ColorHelper.MidnightestBlue, null);
            titleConsole.Print(21, 3, "MOVING CASTLES");
            titleConsole.DefaultBackground = ColorHelper.MidnightestBlue;

            var menuConsole = new ControlsConsole(160, 33)
            {
                Position = new Point(0, 12),
            };

            var continueButton = new SadConsole.Controls.Button(25, 1)
            {
                Text = "Dungeon mode testarea",
                Position = new Point(76, 8),
            };
            continueButton.Click += (_, __) => gameManager.StartDungeonModeDemo();
            menuConsole.Add(continueButton);

            var newGameButton = new SadConsole.Controls.Button(25, 1)
            {
                Text = "Castle mode testarea",
                Position = new Point(76, 10),
            };

            newGameButton.Click += (_, __) => gameManager.StartCastleModeDemo();
            menuConsole.Add(newGameButton);

            var exitButton = new SadConsole.Controls.Button(25, 1)
            {
                Text = "Exit",
                Position = new Point(76, 12),
            };
            exitButton.Click += (_, __) => SadConsole.Game.Instance.Exit();
            menuConsole.Add(exitButton);

            Children.Add(titleConsole);
            Children.Add(menuConsole);
        }
    }
}

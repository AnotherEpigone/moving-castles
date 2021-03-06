﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.GameSystems;
using MovingCastles.Ui.Controls;
using SadConsole;

namespace MovingCastles.Ui.Windows
{
    public class PopupMenuWindow : McControlWindow
    {
        private bool _escReleased;

        public PopupMenuWindow(IUiManager uiManager, IGameManager gameManager)
            : base(36, 7)
        {
            CloseOnEscKey = false; // it would close as soon as it opens...
            IsModalDefault = true;
            Center();

            var background = new Console(Width, Height);
            background.Fill(null, ColorHelper.DarkGreyHighlight, null);

            Children.Add(background);

            const string mainMenuText = "Save and Exit to Main Menu";
            var mainMenuButtonWidth = mainMenuText.Length + 4;
            var mainMenuButton = new McSelectionButton(mainMenuButtonWidth, 1)
            {
                Text = mainMenuText,
                Position = new Point((Width / 2) - (mainMenuButtonWidth / 2), Height - 6),
            };
            mainMenuButton.Click += (_, __) =>
            {
                gameManager.Save();
                Hide();
                uiManager.ShowMainMenu(gameManager);
            };

            const string quitText = "Save and Exit to Desktop";
            var quitButtonWidth = mainMenuText.Length + 4;
            var quitButton = new McSelectionButton(quitButtonWidth, 1)
            {
                Text = quitText,
                Position = new Point((Width / 2) - (quitButtonWidth / 2), Height - 4),
            };
            quitButton.Click += (_, __) =>
            {
                gameManager.Save();
                SadConsole.Game.Instance.Exit();
            };

            const string closeText = "Close";
            var closeButtonWidth = closeText.Length + 4;
            var closeButton = new McSelectionButton(closeButtonWidth, 1)
            {
                Text = closeText,
                Position = new Point((Width / 2) - (closeButtonWidth / 2), Height - 2),
            };
            closeButton.Click += (_, __) => Hide();

            SetupSelectionButtons(mainMenuButton, quitButton, closeButton);
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

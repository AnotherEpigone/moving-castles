using Microsoft.Xna.Framework;
using MovingCastles.Text;
using MovingCastles.Ui.Controls;
using SadConsole;
using SadConsole.Controls;
using System;
using System.Collections.Generic;

namespace MovingCastles.Ui.Windows
{
    public class CommandWindow : McControlWindow
    {
        private const int CommandButtonWidth = 40;

        private readonly Button _closeButton;
        private readonly SadConsole.Console _descriptionArea;

        public CommandWindow()
           : base(90, 30)
        {
            Center();
            CloseOnEscKey = true;
            IsFocused = true;

            // child console
            _descriptionArea = new SadConsole.Console(Width - CommandButtonWidth - 3, Height - 4)
            {
                Position = new Point(CommandButtonWidth + 2, 1),
                DefaultBackground = ColorHelper.ControlBackDark,
            };
            _descriptionArea.Fill(null, ColorHelper.ControlBackDark, null);

            Children.Add(_descriptionArea);

            // buttons
            const string closeText = "Close";
            var closeButtonWidth = closeText.Length + 4;
            _closeButton = new Button(closeButtonWidth)
            {
                Text = closeText,
                Position = new Point(CommandButtonWidth + 1, Height - 2),
            };
            _closeButton.Click += (_, __) => Hide();

            // commands
            var commandButtons = new Dictionary<McSelectionButton, Action>();
            var summaryButton = new McSelectionButton(CommandButtonWidth - 1, 1)
            {
                Text = TextHelper.TruncateString("Commands overview", CommandButtonWidth - 5),
                Position = new Point(0, 1),
            };
            Action summaryButtonAction = () => ShowDescription(Gui.Command_Overview);
            commandButtons.Add(summaryButton, summaryButtonAction);

            var movementButton = new McSelectionButton(CommandButtonWidth - 1, 1)
            {
                Text = TextHelper.TruncateString("Movement", CommandButtonWidth - 5),
                Position = new Point(0, 2),
            };
            Action movementButtonAction = () => ShowDescription(Gui.Command_Movement);
            commandButtons.Add(movementButton, movementButtonAction);

            var waitButton = new McSelectionButton(CommandButtonWidth - 1, 1)
            {
                Text = TextHelper.TruncateString("Wait", CommandButtonWidth - 5),
                Position = new Point(0, 3),
            };
            Action waitButtonAction = () => ShowDescription(Gui.Command_Wait);
            commandButtons.Add(waitButton, waitButtonAction);
            
            var interactButton = new McSelectionButton(CommandButtonWidth - 1, 1)
            {
                Text = TextHelper.TruncateString("Interact", CommandButtonWidth - 5),
                Position = new Point(0, 4),
            };
            Action interactButtonAction = () => ShowDescription(Gui.Command_Interact);
            commandButtons.Add(interactButton, interactButtonAction);

            Add(_closeButton);
            SetupSelectionButtons(commandButtons);
        }

        public new void Show()
        {
            _descriptionArea.Clear();
            base.Show(true);
        }

        private void ShowDescription(string description)
        {
            _descriptionArea.Clear();
            _descriptionArea.Cursor.Position = new Point(0, 0);
            _descriptionArea.Cursor.Print(
                new ColoredString(
                    description,
                    new Cell(_descriptionArea.DefaultForeground, _descriptionArea.DefaultBackground)));
        }
    }
}

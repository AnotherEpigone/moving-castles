using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace MovingCastles.Ui.Windows
{
    public class CommandWindow : McControlWindow
    {
        private const int CommandButtonWidth = 40;

        private readonly Button _closeButton;
        private readonly Console _descriptionArea;

        public CommandWindow()
           : base(90, 30)
        {
            Center();
            CloseOnEscKey = true;
            IsFocused = true;

            // child console
            _descriptionArea = new Console(Width - CommandButtonWidth - 3, Height - 4)
            {
                Position = new Point(CommandButtonWidth + 2, 1),
                DefaultBackground = ColorHelper.MidnighterBlue,
            };
            _descriptionArea.Fill(null, ColorHelper.MidnighterBlue, null);

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

            Add(_closeButton);
        }

        public new void Show()
        {
            base.Show(true);
        }
    }
}

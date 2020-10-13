using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace MovingCastles.Ui.Windows
{
    public class SpellSelectionWindow : Window
    {
        private readonly Button _castButton;
        private readonly Button _cancelButton;
        private readonly Console _descriptionArea;

        public SpellSelectionWindow()
            : base(120, 30)
        {
            const int spellButtonWidth = 40;
            CloseOnEscKey = true;

            Center();

            const string castText = "Cast";
            var castButtonWidth = castText.Length + 4;
            _castButton = new Button(castButtonWidth)
            {
                Text = castText,
                Position = new Point(Width - castButtonWidth, Height - 2),
            };

            const string cancelText = "Cancel";
            var cancelButtonWidth = cancelText.Length + 4;
            _cancelButton = new Button(cancelButtonWidth)
            {
                Text = cancelText,
                Position = new Point(spellButtonWidth + 1, Height - 2),
            };
            _cancelButton.Click += (_, __) => Hide();

            _descriptionArea = new Console(Width - spellButtonWidth - 3, Height - 4)
            {
                Position = new Point(spellButtonWidth + 2, 1),
                DefaultBackground = ColorHelper.MidnighterBlue,
            };
            _descriptionArea.Fill(null, ColorHelper.MidnighterBlue, null);

            Children.Add(_descriptionArea);

            ////Closed += (_, __) => _selectedItem = null;
        }
    }
}

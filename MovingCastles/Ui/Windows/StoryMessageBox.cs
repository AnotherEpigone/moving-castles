using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace MovingCastles.Ui.Windows
{
    public class StoryMessageBox : McControlWindow
    {
        private readonly Console _descriptionArea;

        public StoryMessageBox(string message)
            : this(message, 80, 30) { }

        public StoryMessageBox(string message, int width, int height)
            : base(width, height)
        {
            _descriptionArea = new Console(Width - 4, Height - 4)
            {
                Position = new Point(2, 1),
                DefaultBackground = ColorHelper.MidnighterBlue,
            };
            _descriptionArea.Fill(null, ColorHelper.MidnighterBlue, null);

            _descriptionArea.Cursor.Position = new Point(0, 0);
            _descriptionArea.Cursor.Print(new ColoredString(
                    message,
                    new Cell(_descriptionArea.DefaultForeground, _descriptionArea.DefaultBackground)));

            var closeButton = new Button(9)
            {
                Text = "Close",
                Position = new Point(width - 11, height - 2),
            };
            closeButton.Click += (_, __) => Hide();

            Children.Add(_descriptionArea);

            Add(closeButton);

            CloseOnEscKey = true;
            Center();
        }
    }
}

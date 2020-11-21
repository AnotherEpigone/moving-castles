using Microsoft.Xna.Framework;
using SadConsole;

namespace MovingCastles.Ui.Windows
{
    public class StoryMessageBox : McControlWindow
    {
        private readonly Console _descriptionArea;

        public StoryMessageBox(string message)
            : this(message, 120, 40) { }

        public StoryMessageBox(string message, int width, int height)
            : base(width, height)
        {
            _descriptionArea = new Console(Width - 2, Height - 2)
            {
                Position = new Point(1, 1),
                DefaultBackground = ColorHelper.MidnighterBlue,
            };
            _descriptionArea.Fill(null, ColorHelper.MidnighterBlue, null);

            _descriptionArea.Cursor.Position = new Point(0, 0);
            _descriptionArea.Cursor.Print(message);

            Children.Add(_descriptionArea);

            CloseOnEscKey = true;
            Center();
        }
    }
}

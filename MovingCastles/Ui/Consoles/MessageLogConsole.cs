using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;

namespace MovingCastles.Ui
{
    public class MessageLogConsole : ContainerConsole
    {
        private const int _maxLines = 50;

        private readonly Queue<string> _lines;
        private readonly ScrollingConsole _messageConsole;

        public MessageLogConsole(int width, int height, Font font)
        {
            _lines = new Queue<string>();
            _messageConsole = new ScrollingConsole(width, height, font)
            {
                DefaultBackground = ColorHelper.MidnightestBlue
            };

            Children.Add(_messageConsole);
        }

        public void Add(string message)
        {
            _lines.Enqueue(message);
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }

            var coloredMessage = new ColoredString($"> {message}\r\n", new Cell(Color.Gainsboro, ColorHelper.MidnightestBlue));
            _messageConsole.Cursor.Print(coloredMessage);
        }
    }
}

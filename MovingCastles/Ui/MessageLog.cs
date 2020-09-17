using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Ui
{
    public class MessageLog : ContainerConsole
    {
        private const int _maxLines = 50;

        private readonly Queue<string> _lines;
        private readonly ScrollingConsole _messageConsole;

        public MessageLog(int width, int height, Font font)
        {
            _lines = new Queue<string>();

            // add the message console, reposition, and add it to the window
            _messageConsole = new ScrollingConsole(width, height, font);
            _messageConsole.DefaultBackground = ColorHelper.MidnightestBlue;
            Children.Add(_messageConsole);
        }

        public void Add(string message)
        {
            _lines.Enqueue(message);
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }

            // Move the cursor to the last line and print the message.
            _messageConsole.Cursor.Position = new Point(1, _lines.Count - 1);

            var coloredMessage = new ColoredString(message + "\n", new Cell(Color.Gainsboro, ColorHelper.MidnightestBlue));
            _messageConsole.Cursor.Print(coloredMessage);
        }
    }
}

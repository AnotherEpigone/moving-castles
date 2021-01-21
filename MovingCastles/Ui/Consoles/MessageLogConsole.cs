using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MovingCastles.Ui
{
    public class MessageLogConsole : ContainerConsole
    {
        private const int _maxLines = 50;

        private readonly Queue<string> _lines;
        private readonly ScrollingConsole _messageConsole;
        private readonly Color _textBackground;
        private readonly System.TimeSpan _hideAfter;
        private readonly object _hideTimerLock;

        private Task _hideTask;
        private CancellationTokenSource _hideTaskCancelTokenSource;

        public MessageLogConsole(int width, int height, Font font)
            : this(width, height, font, ColorHelper.ControlBack, ColorHelper.SelectedBackground, System.TimeSpan.Zero)
        {
        }

        public MessageLogConsole(
            int width,
            int height,
            Font font,
            Color consoleBackground,
            Color textBackground,
            System.TimeSpan hideAfter)
        {
            Height = height;

            _hideTimerLock = new object();
            _lines = new Queue<string>();
            _messageConsole = new ScrollingConsole(width, height, font)
            {
                DefaultBackground = consoleBackground,
            };

            Children.Add(_messageConsole);
            _textBackground = textBackground;
            _hideAfter = hideAfter;
        }

        public new int Height { get; }

        public void ShowTemp()
        {
            IsVisible = true;
            lock (_hideTimerLock)
            {
                if (_hideTask != null)
                {
                    _hideTaskCancelTokenSource.Cancel();
                }

                _hideTaskCancelTokenSource = new CancellationTokenSource();
                _hideTask = Task.Delay(_hideAfter, _hideTaskCancelTokenSource.Token)
                .ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        lock (_hideTimerLock)
                        {
                            if (!t.IsCanceled)
                            {
                                IsVisible = false;
                                _hideTask = null;
                            }
                        }
                    }
                });
            }
        }

        public void Add(string message, bool highlight)
        {
            if (_hideAfter != System.TimeSpan.Zero)
            {
                if (!IsVisible)
                {
                    _messageConsole.Clear();
                    _messageConsole.Cursor.Position = new Point(0, 0);
                }

                ShowTemp();
            }

            _lines.Enqueue(message);
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }

            var backgroundColor = highlight
                ? ColorHelper.SelectedBackground
                : _textBackground;

            var coloredMessage = new ColoredString($"> {message}\r\n", new Cell(Color.Gainsboro, backgroundColor));
            _messageConsole.Cursor.Print(coloredMessage);
        }
    }
}

using Microsoft.Xna.Framework;
using MovingCastles.GameSystems.Journal;
using MovingCastles.Ui.Controls;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Ui.Windows
{
    public class JournalWindow : McControlWindow
    {
        private readonly Console _entriesArea;
        private readonly int _topicButtonWidth;
        private readonly Button _closeButton;
        private ILookup<string, JournalEntry> _entries;

        public JournalWindow(int width, int height)
            : base(width, height)
        {
            CloseOnEscKey = true;
            IsModalDefault = true;
            Center();

            _topicButtonWidth = width / 3;

            ////const string locationText = "Location (L)";
            ////var locationButtonWidth = locationText.Length + 4;
            ////var locationButton = new Button(locationButtonWidth)
            ////{
            ////    Text = locationText,
            ////    IsEnabled = false,
            ////    Position = new Point((width / 3) - (locationButtonWidth / 2), 0),
            ////};

            ////const string notesText = "Notes (N)";
            ////var notesButtonWidth = notesText.Length + 4;
            ////var notesButton = new Button(notesButtonWidth)
            ////{
            ////    Text = notesText,
            ////    Position = new Point((width * 2 / 3) - (notesButtonWidth / 2), 0),
            ////};

            _closeButton = new Button(9)
            {
                Text = "Close",
                Position = new Point(width - 11, height - 2),
            };
            _closeButton.Click += (_, __) => Hide();

            _entriesArea = new Console(width - _topicButtonWidth - 3, height - 4)
            {
                Position = new Point(_topicButtonWidth + 2, 1),
                DefaultBackground = ColorHelper.MidnighterBlue,
            };
            _entriesArea.Fill(null, ColorHelper.MidnighterBlue, null);

            Children.Add(_entriesArea);
            ////Add(locationButton);
            ////Add(notesButton);
            Add(_closeButton);
        }

        public void Show(ILookup<string, JournalEntry> entries)
        {
            _entries = entries;
            RefreshControls(BuildTopicControls(entries.Select(e => e.Key)));

            base.Show(true);
        }

        private Dictionary<McSelectionButton, System.Action> BuildTopicControls(IEnumerable<string> topics)
        {
            var yCount = 0;
            return topics.ToDictionary(
                topic =>
                {
                    return new McSelectionButton(_topicButtonWidth - 1, 1)
                    {
                        Text = TextHelper.TruncateString(topic, _topicButtonWidth - 5),
                        Position = new Point(0, yCount++),
                    };
                },
                topic => (System.Action)(() => OnTopicSelected(topic)));
        }

        private void OnTopicSelected(string topic)
        {
            _entriesArea.Clear();
            _entriesArea.Cursor.Position = new Point(0, 0);
            _entriesArea.Cursor.Print(
                GetStringForEntries(_entries[topic]));
        }

        private ColoredString GetStringForEntries(IEnumerable<JournalEntry> entries)
        {
            return new ColoredString("entries here");
        }

        private void RefreshControls(Dictionary<McSelectionButton, System.Action> buttons)
        {
            RemoveAll();

            Add(_closeButton);

            SetupSelectionButtons(buttons);
        }
    }
}

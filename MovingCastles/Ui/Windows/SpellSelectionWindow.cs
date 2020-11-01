using Microsoft.Xna.Framework;
using MovingCastles.GameSystems.Spells;
using MovingCastles.Ui.Controls;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Ui.Windows
{
    public class SpellSelectionWindow : McControlWindow
    {
        private const int SpellButtonWidth = 40;
        private readonly Button _castButton;
        private readonly Button _cancelButton;
        private readonly Console _descriptionArea;
        private readonly Dictionary<char, SpellTemplate> _hotkeys;
        private SpellTemplate _selectedSpell;
        private System.Action<SpellTemplate> _onCast;

        public SpellSelectionWindow()
            : base(120, 30)
        {
            _hotkeys = new Dictionary<char, SpellTemplate>();

            CloseOnEscKey = true;

            Center();
            IsFocused = true;

            const string castText = "Cast";
            var castButtonWidth = castText.Length + 4;
            _castButton = new Button(castButtonWidth)
            {
                Text = castText,
                Position = new Point(Width - castButtonWidth, Height - 2),
            };
            _castButton.Click += (_, __) =>
            {
                _onCast?.Invoke(_selectedSpell);
                Hide();
            };

            const string cancelText = "Cancel";
            var cancelButtonWidth = cancelText.Length + 4;
            _cancelButton = new Button(cancelButtonWidth)
            {
                Text = cancelText,
                Position = new Point(SpellButtonWidth + 1, Height - 2),
            };
            _cancelButton.Click += (_, __) => Hide();

            _descriptionArea = new Console(Width - SpellButtonWidth - 3, Height - 4)
            {
                Position = new Point(SpellButtonWidth + 2, 1),
                DefaultBackground = ColorHelper.MidnighterBlue,
            };
            _descriptionArea.Fill(null, ColorHelper.MidnighterBlue, null);

            Children.Add(_descriptionArea);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            foreach (var key in info.KeysPressed)
            {
                if (_hotkeys.TryGetValue(key.Character, out var spell))
                {
                    _onCast(spell);
                    Hide();
                    return true;
                }
            }

            return base.ProcessKeyboard(info);
        }

        public void Show(IEnumerable<SpellTemplate> spells, System.Action<SpellTemplate> onCast)
        {
            _onCast = onCast;
            _selectedSpell = null;

            _castButton.IsEnabled = false;

            RefreshControls(spells);

            base.Show(true);
        }

        private Dictionary<McSelectionButton, System.Action> BuildSpellControls(IEnumerable<SpellTemplate> spells)
        {
            _hotkeys.Clear();

            var yCount = 0;
            var controlDictionary = spells
                .OrderBy(s => s.Name)
                .ToDictionary(
                    i =>
                    {
                        var hotkeyLetter = (char)('a' + yCount);
                        _hotkeys.Add(hotkeyLetter, i);

                        var spellButton = new McSelectionButton(SpellButtonWidth - 1, 1)
                        {
                            Text = TextHelper.TruncateString($"{hotkeyLetter}. {i.Name}", SpellButtonWidth - 5),
                            Position = new Point(0, yCount++),
                        };
                        spellButton.Click += (_, __) =>
                        {
                            _onCast(i);
                            Hide();
                        };
                        return spellButton;
                    },
                    i => (System.Action)(() => OnSpellSelected(i)));

            var buttons = controlDictionary.Keys.ToArray();
            for (int i = 1; i < buttons.Length; i++)
            {
                buttons[i - 1].NextSelection = buttons[i];
                buttons[i].PreviousSelection = buttons[i - 1];
            }

            return controlDictionary;
        }

        private void OnSpellSelected(SpellTemplate spell)
        {
            _selectedSpell = spell;
            _descriptionArea.Clear();
            _descriptionArea.Cursor.Position = new Point(0, 0);
            _descriptionArea.Cursor.Print(
                new ColoredString(
                    _selectedSpell?.Description ?? string.Empty,
                    new Cell(_descriptionArea.DefaultForeground, _descriptionArea.DefaultBackground)));
            _castButton.IsEnabled = true;
        }

        private void RefreshControls(IEnumerable<SpellTemplate> spells)
        {
            RemoveAll();

            Add(_castButton);
            Add(_cancelButton);

            SetupSelectionButtons(BuildSpellControls(spells));
        }
    }
}

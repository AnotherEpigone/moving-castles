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
        private float _availableEndowment;

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
                DefaultBackground = ColorHelper.ControlBackDark,
            };
            _descriptionArea.Fill(null, ColorHelper.ControlBackDark, null);

            Children.Add(_descriptionArea);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            foreach (var key in info.KeysPressed)
            {
                if (_hotkeys.TryGetValue(key.Character, out var spell)
                    && _availableEndowment >= spell.EndowmentCost)
                {
                    _onCast(spell);
                    Hide();
                    return true;
                }
            }

            return base.ProcessKeyboard(info);
        }

        public void Show(
            IEnumerable<SpellTemplate> spells,
            System.Action<SpellTemplate> onCast,
            float availableEndowment)
        {
            _availableEndowment = availableEndowment;
            _onCast = onCast;
            _selectedSpell = null;
            _descriptionArea.Clear();

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
                    s =>
                    {
                        var hotkeyLetter = (char)('a' + yCount);
                        _hotkeys.Add(hotkeyLetter, s);

                        var spellButton = new McSelectionButton(SpellButtonWidth - 1, 1)
                        {
                            Text = TextHelper.TruncateString($"{hotkeyLetter}. {s.Name}", SpellButtonWidth - 5),
                            Position = new Point(0, yCount++),
                            IsEnabled = _availableEndowment >= s.EndowmentCost,
                        };
                        spellButton.Click += (_, __) =>
                        {
                            _onCast(s);
                            Hide();
                        };
                        return spellButton;
                    },
                    s => (System.Action)(() => OnSpellSelected(s)));

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
                    GetSpellDescription(_selectedSpell),
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

        public string GetSpellDescription(SpellTemplate spell)
        {
            var desc = spell?.Description ?? string.Empty;
            var stats = $"Endowment cost: {spell.EndowmentCost}\r\nRange: {spell.TargettingStyle.Range}";
            var effects = string.Join("\r\n", spell.Effects.Select(e => e.Description));
            return $"{desc}\r\n\n{stats}\r\n\n{effects}";
        }
    }
}

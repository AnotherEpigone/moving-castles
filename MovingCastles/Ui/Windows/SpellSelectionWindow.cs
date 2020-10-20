﻿using Microsoft.Xna.Framework;
using MovingCastles.GameSystems.Spells;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Ui.Windows
{
    public class SpellSelectionWindow : Window
    {
        private const int SpellButtonWidth = 40;
        private readonly Button _castButton;
        private readonly Button _cancelButton;
        private readonly Console _descriptionArea;
        private readonly Dictionary<char, SpellTemplate> _hotkeys;
        private SpellTemplate _selectedSpell;
        private SelectionButton _lastFocusedButton;
        private Dictionary<ControlBase, SpellTemplate> _spellButtons;
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

        public override void Update(System.TimeSpan time)
        {
            if (!(FocusedControl is SelectionButton focusedButton)
                || focusedButton == _lastFocusedButton)
            {
                base.Update(time);
                return;
            }

            _lastFocusedButton = focusedButton;
            OnSpellSelected(_spellButtons[focusedButton]);

            base.Update(time);
        }

        public void Show(IEnumerable<SpellTemplate> spells, System.Action<SpellTemplate> onCast)
        {
            _onCast = onCast;
            _selectedSpell = null;

            _castButton.IsEnabled = false;

            _spellButtons = BuildSpellControls(spells);
            RefreshControls(_spellButtons.Keys);

            base.Show(true);
        }

        private Dictionary<ControlBase, SpellTemplate> BuildSpellControls(IEnumerable<SpellTemplate> spells)
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

                        var spellButton = new SelectionButton(SpellButtonWidth - 1, 1)
                        {
                            Text = TextHelper.TruncateString($"{hotkeyLetter}. {i.Name}", SpellButtonWidth - 5),
                            Position = new Point(0, yCount++),
                        };
                        spellButton.MouseEnter += (_, __) => OnSpellSelected(i);
                        spellButton.Click += (_, __) => _onCast(i);
                        return (ControlBase)spellButton;
                    },
                    i => i);

            var buttons = controlDictionary.Keys.OfType<SelectionButton>().ToArray();
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

        private void RefreshControls(IEnumerable<ControlBase> controls)
        {
            RemoveAll();

            Add(_castButton);
            Add(_cancelButton);

            foreach (var control in controls)
            {
                Add(control);
            }

            FocusedControl = controls.FirstOrDefault();
        }
    }
}

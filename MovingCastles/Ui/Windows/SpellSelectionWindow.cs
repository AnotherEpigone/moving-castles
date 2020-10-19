﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        private System.Action<SpellTemplate> _onCast;

        public SpellSelectionWindow()
            : base(120, 30)
        {
            _hotkeys = new Dictionary<char, SpellTemplate>();

            CloseOnEscKey = true;

            Center();

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

            var controls = BuildSpellControls(spells);
            RefreshControls(controls);

            base.Show(true);
        }

        private List<ControlBase> BuildSpellControls(IEnumerable<SpellTemplate> spells)
        {
            _hotkeys.Clear();

            var yCount = 0;
            return spells
                .OrderBy(s => s.Name)
                .Select(i =>
                {
                    var hotkeyLetter = (char)('a' + yCount);
                    _hotkeys.Add(hotkeyLetter, i);

                    var spellButton = new Button(SpellButtonWidth - 1)
                    {
                        Text = TextHelper.TruncateString($"{hotkeyLetter}. {i.Name}", SpellButtonWidth - 5),
                        Position = new Point(0, yCount++),
                    };
                    spellButton.Click += (_, __) =>
                    {
                        _selectedSpell = i;
                        _descriptionArea.Clear();
                        _descriptionArea.Cursor.Position = new Point(0, 0);
                        _descriptionArea.Cursor.Print(
                            new ColoredString(
                                _selectedSpell?.Description ?? string.Empty,
                                new Cell(_descriptionArea.DefaultForeground, _descriptionArea.DefaultBackground)));
                        _castButton.IsEnabled = true;
                    };
                    return spellButton;
                })
                .ToList<ControlBase>();
        }

        private void RefreshControls(List<ControlBase> controls)
        {
            RemoveAll();

            Add(_castButton);
            Add(_cancelButton);

            foreach (var control in controls)
            {
                Add(control);
            }
        }
    }
}
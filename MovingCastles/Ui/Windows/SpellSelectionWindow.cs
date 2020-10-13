using Microsoft.Xna.Framework;
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
        private SpellTemplate _selectedSpell;

        public SpellSelectionWindow()
            : base(120, 30)
        {
            CloseOnEscKey = true;

            Center();

            const string castText = "Cast";
            var castButtonWidth = castText.Length + 4;
            _castButton = new Button(castButtonWidth)
            {
                Text = castText,
                Position = new Point(Width - castButtonWidth, Height - 2),
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

            Closed += (_, __) => _selectedSpell = null;
        }

        public void Show(IEnumerable<SpellTemplate> spells)
        {
            var controls = BuildSpellControls(spells);
            RefreshControls(controls);

            base.Show(true);
        }

        private List<ControlBase> BuildSpellControls(IEnumerable<SpellTemplate> spells)
        {
            var yCount = 0;
            return spells.Select(i =>
                {
                    var spellButton = new Button(SpellButtonWidth - 1)
                    {
                        Text = TextHelper.TruncateString(i.Name, SpellButtonWidth - 5),
                        Position = new Point(0, yCount++),
                    };
                    spellButton.Click += (_, __) =>
                    {
                        _selectedSpell = i;
                        _descriptionArea.Clear();
                        _descriptionArea.Cursor.Position = new Point(0, 0);
                        _descriptionArea.Cursor.Print(_selectedSpell.Description);
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

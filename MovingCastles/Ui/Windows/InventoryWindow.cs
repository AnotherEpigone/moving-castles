using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.GameSystems.Items;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace MovingCastles.Ui.Windows
{
    public class InventoryWindow : McControlWindow
    {
        private readonly Console _descriptionArea;
        private readonly Button _useButton;
        private readonly Button _closeButton;
        private readonly int _itemButtonWidth;
        private ItemTemplate _selectedItem;

        public InventoryWindow(int width, int height)
            : base(width, height)
        {
            Contract.Requires(width > 40, "Menu width must be > 40");
            Contract.Requires(width > 10, "Menu width must be > 10");

            _itemButtonWidth = width / 3;
            CloseOnEscKey = true;

            Center();

            _useButton = new Button(7)
            {
                Text = "Use",
                Position = new Point(_itemButtonWidth + 2, height - 2),
            };

            _closeButton = new Button(9)
            {
                Text = "Close",
                Position = new Point(width - 9, height - 2),
            };
            _closeButton.Click += (_, __) => Hide();

            _descriptionArea = new Console(width - _itemButtonWidth - 3, height - 4)
            {
                Position = new Point(_itemButtonWidth + 2, 1),
                DefaultBackground = ColorHelper.MidnighterBlue,
            };
            _descriptionArea.Fill(null, ColorHelper.MidnighterBlue, null);

            Children.Add(_descriptionArea);

            Closed += (_, __) => _selectedItem = null;
        }

        public void Show(IInventoryComponent inventory)
        {
            RefreshControls(BuildItemControls(inventory.Items));

            base.Show(true);
        }

        private void OnItemSelected(ItemTemplate item)
        {
            _selectedItem = item;
            _descriptionArea.Clear();
            _descriptionArea.Cursor.Position = new Point(0, 0);
            _descriptionArea.Cursor.Print(
                new ColoredString(
                    _selectedItem?.Description ?? string.Empty,
                    new Cell(_descriptionArea.DefaultForeground, _descriptionArea.DefaultBackground)));
        }

        private Dictionary<SelectionButton, System.Action> BuildItemControls(IEnumerable<ItemTemplate> items)
        {
            var yCount = 0;
            return items.ToDictionary(
                i =>
                {
                    return new SelectionButton(_itemButtonWidth - 1, 1)
                    {
                        Text = TextHelper.TruncateString(i.Name, _itemButtonWidth - 5),
                        Position = new Point(0, yCount++),
                    };
                },
                i => (System.Action)(() => OnItemSelected(i)));
        }

        private void RefreshControls(Dictionary<SelectionButton, System.Action> buttons)
        {
            RemoveAll();

            Add(_useButton);
            Add(_closeButton);

            SetupSelectionButtons(buttons);
        }
    }
}

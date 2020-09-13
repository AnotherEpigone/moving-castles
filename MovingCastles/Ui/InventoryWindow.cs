using MovingCastles.Components;
using MovingCastles.GameSystems.Items;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace MovingCastles.Ui
{
    public class InventoryWindow : Window
    {
        private readonly DrawingSurface _descriptionArea;
        private readonly Button _useButton;
        private readonly Button _closeButton;
        private readonly int _itemButtomWidth;
        private string _selectedItemDesc;

        public InventoryWindow(int width, int height)
            : base(width, height)
        {
            Contract.Requires(width > 40, "Menu width must be > 200");
            Contract.Requires(width > 10, "Menu width must be > 100");

            _selectedItemDesc = "description";
            _itemButtomWidth = width / 3;

            Center();

            _useButton = new Button(7)
            {
                Text = "Use",
                Position = new Microsoft.Xna.Framework.Point(_itemButtomWidth + 3, height - 2),
            };

            _closeButton = new Button(9)
            {
                Text = "Close",
                Position = new Microsoft.Xna.Framework.Point(width - 9, height - 2),
            };
            _closeButton.Click += (_, __) => Hide();

            CloseOnEscKey = true;

            _descriptionArea = new DrawingSurface(_itemButtomWidth, height - 3)
            {
                Position = new Microsoft.Xna.Framework.Point(_itemButtomWidth, 0),
                OnDraw = ds =>
                {
                    if (!ds.IsDirty)
                    {
                        return;
                    }

                    ds.Surface.Fill(null, UiManager.MidnighterBlue, null);
                    ds.Surface.Print(2, 2, _selectedItemDesc);
                    ds.IsDirty = true;
                },
            };
        }

        public void Show(IInventoryComponent inventory)
        {
            var controls = BuildItemControls(inventory.Items);
            RefreshControls(controls);

            base.Show(true);
        }

        private List<ControlBase> BuildItemControls(IEnumerable<IInventoryItem> items)
        {
            var yCount = 0;
            return items.Select(i => new Button(_itemButtomWidth)
            {
                Text = TruncateName(i.Name, _itemButtomWidth - 4),
                Position = new Microsoft.Xna.Framework.Point(0, yCount++),
            })
                .ToList<ControlBase>();
        }

        private void RefreshControls(List<ControlBase> controls)
        {
            foreach (var existingControl in Controls.ToList())
            {
                Remove(existingControl);
            }

            Add(_descriptionArea);
            Add(_useButton);
            Add(_closeButton);

            foreach (var control in controls)
            {
                Add(control);
            }
        }

        private string TruncateName(string name, int maxLen)
        {
            if (name.Length > maxLen)
            {
                return name.Substring(0, maxLen - 3) + "...";
            }

            return name;
        }
    }
}

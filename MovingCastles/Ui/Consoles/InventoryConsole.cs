using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Items;
using MovingCastles.Ui.Controls;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Entities;

namespace MovingCastles.Ui.Consoles
{
    public class InventoryConsole : McControlsConsole
    {
        private readonly IDungeonMaster _dungeonMaster;
        private readonly Console _descriptionArea;
        private readonly Button _useButton;
        private readonly Button _dropButton;
        private readonly Button _equipButton;
        private readonly Button _closeButton;
        private readonly int _itemButtonWidth;
        private readonly ILogManager _logManager;

        private Item _selectedItem;
        private IInventoryComponent _inventory;
        private IEquipmentComponent _equipment;
        private System.Action _hideCallback;

        public InventoryConsole(
            int width,
            int height,
            IDungeonMaster dungeonMaster,
            ILogManager logManager)
            : base(width, height)
        {
            Contract.Requires(width > 40, "Menu width must be > 40");
            Contract.Requires(width > 10, "Menu width must be > 10");

            _dungeonMaster = dungeonMaster;
            _logManager = logManager;

            IsVisible = false;

            _itemButtonWidth = width / 3;

            _useButton = new Button(7)
            {
                Text = "Use",
                IsEnabled = false,
                Position = new Point(2, height - 2),
            };

            _dropButton = new Button(12)
            {
                Text = "Drop (D)",
                Position = new Point(_useButton.Position.X + _useButton.Width + 2, height - 2),
            };
            _dropButton.Click += (_, __) => Drop();

            _equipButton = new Button(13)
            {
                Text = "Equip (E)",
                Position = new Point(_dropButton.Position.X + _dropButton.Width + 2, height - 2),
            };
            _equipButton.Click += (_, __) => Equip();

            _closeButton = new Button(15)
            {
                Text = "Close (Esc)",
                Position = new Point(width - 16, height - 2),
            };
            _closeButton.Click += (_, __) => _hideCallback();

            _descriptionArea = new Console(width - _itemButtonWidth - 3, height - 4)
            {
                Position = new Point(_itemButtonWidth + 2, 1),
                DefaultBackground = ColorHelper.ControlBackDark,
            };
            _descriptionArea.Fill(null, ColorHelper.ControlBackDark, null);

            Children.Add(_descriptionArea);
        }

        public void Hide()
        {
            IsVisible = false;
            _selectedItem = null;

            _inventory.ContentsChanged -= Inventory_ContentsChanged;
        }

        public void Show(
            IInventoryComponent inventory,
            IEquipmentComponent equipment,
            System.Action hideCallback)
        {
            _hideCallback = hideCallback;
            _inventory = inventory;
            _equipment = equipment;
            RefreshControls(BuildItemControls(_inventory.GetItems()));

            _inventory.ContentsChanged += Inventory_ContentsChanged;

            IsVisible = true;
        }

        private void Inventory_ContentsChanged(object sender, System.EventArgs e)
        {
            RefreshControls(BuildItemControls(_inventory.GetItems()));
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Escape))
            {
                _hideCallback();
                return true;
            }

            if (info.IsKeyPressed(Keys.D)
                && _dropButton.IsEnabled)
            {
                Drop();
                return true;
            }

            if (info.IsKeyPressed(Keys.E)
                && _equipButton.IsEnabled)
            {
                Equip();
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        private void OnItemSelected(Item item)
        {
            _selectedItem = item;

            _dropButton.IsEnabled = true;
            var categoryId = ItemAtlas.ItemsById[_selectedItem.TemplateId].EquipCategoryId;
            _equipButton.IsEnabled = _equipment.CanEquip(_selectedItem, categoryId);

            _descriptionArea.Clear();
            _descriptionArea.Cursor.Position = new Point(0, 0);
            _descriptionArea.Cursor.Print(
                new ColoredString(
                    _selectedItem?.Description ?? string.Empty,
                    new Cell(_descriptionArea.DefaultForeground, _descriptionArea.DefaultBackground)));
        }

        private Dictionary<McSelectionButton, System.Action> BuildItemControls(IEnumerable<Item> items)
        {
            var yCount = 0;
            return items.ToDictionary(
                i =>
                {
                    return new McSelectionButton(_itemButtonWidth - 1, 1)
                    {
                        Text = TextHelper.TruncateString(i.Name, _itemButtonWidth - 5),
                        Position = new Point(0, yCount++),
                    };
                },
                i => (System.Action)(() => OnItemSelected(i)));
        }

        private void Equip()
        {
            var categoryId = ItemAtlas.ItemsById[_selectedItem.TemplateId].EquipCategoryId;
            if (!_equipment.CanEquip(_selectedItem, categoryId))
            {
                return;
            }

            _logManager.StoryLog($"{((McEntity)_inventory.Parent).ColoredName} equipped {_selectedItem.ColoredName}.");

            var item = _selectedItem;
            _inventory.RemoveItem(item, _dungeonMaster, _logManager);
            _equipment.Equip(item, categoryId, _logManager);

            RefreshControls(BuildItemControls(_inventory.GetItems()));
        }

        private void Drop()
        {
            var item = _selectedItem;
            _inventory.RemoveItem(item, _dungeonMaster, _logManager);
            _logManager.StoryLog($"{((McEntity)_inventory.Parent).ColoredName} dropped {item.ColoredName}.");

            var mapConsoleResult = _dungeonMaster.GetCurrentMapConsole();
            if (_dungeonMaster.ModeMaster.Mode == GameMode.Dungeon
                && mapConsoleResult.HasValue)
            {
                var droppedItem = _dungeonMaster.ModeMaster.EntityFactory.CreateItem(_dungeonMaster.Player.Position, item);
                var mapConsole = mapConsoleResult.ValueOr(default(DungeonMapConsole));
                mapConsole.AddEntity(droppedItem);
            }

            RefreshControls(BuildItemControls(_inventory.GetItems()));
        }

        private void RefreshControls(Dictionary<McSelectionButton, System.Action> buttons)
        {
            _selectedItem = null;
            _descriptionArea.Clear();

            RemoveAll();

            Add(_useButton);
            Add(_closeButton);
            Add(_dropButton);
            Add(_equipButton);

            _dropButton.IsEnabled = false;
            _equipButton.IsEnabled = false;

            SetupSelectionButtons(buttons);
        }
    }
}

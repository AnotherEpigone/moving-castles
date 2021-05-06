using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Items;
using MovingCastles.Ui.Consoles;
using MovingCastles.Ui.Controls;
using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Ui.Windows
{
    public class InventoryWindow : McControlWindow
    {
        private readonly IDungeonMaster _dungeonMaster;
        private readonly Console _descriptionArea;
        private readonly Button _useButton;
        private readonly Button _dropButton;
        private readonly Button _closeButton;
        private readonly int _itemButtonWidth;

        private Item _selectedItem;
        private IInventoryComponent _inventory;
        private readonly ILogManager _logManager;

        public InventoryWindow(int width, int height, IDungeonMaster dungeonMaster, ILogManager logManager)
            : base(width, height)
        {
            Contract.Requires(width > 40, "Menu width must be > 40");
            Contract.Requires(width > 10, "Menu width must be > 10");

            _dungeonMaster = dungeonMaster;
            _logManager = logManager;

            _itemButtonWidth = width / 3;
            CloseOnEscKey = true;

            Center();

            _useButton = new Button(7)
            {
                Text = "Use",
                IsEnabled = false,
                Position = new Point(_itemButtonWidth + 2, height - 2),
            };

            _dropButton = new Button(12)
            {
                Text = "Drop (D)",
                Position = new Point(_useButton.Position.X + _useButton.Width + 2, height - 2),
            };
            _dropButton.Click += (_, __) => Drop();

            _closeButton = new Button(15)
            {
                Text = "Close (Esc)",
                Position = new Point(width - 16, height - 2),
            };
            _closeButton.Click += (_, __) => Hide();

            _descriptionArea = new Console(width - _itemButtonWidth - 3, height - 4)
            {
                Position = new Point(_itemButtonWidth + 2, 1),
                DefaultBackground = ColorHelper.ControlBackDark,
            };
            _descriptionArea.Fill(null, ColorHelper.ControlBackDark, null);

            Children.Add(_descriptionArea);

            Closed += (_, __) => _selectedItem = null;
        }

        public void Show(IInventoryComponent inventory)
        {
            _inventory = inventory;
            RefreshControls(BuildItemControls(_inventory.GetItems()));

            base.Show(true);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.D))
            {
                Drop();
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        private void OnItemSelected(Item item)
        {
            _selectedItem = item;
            _dropButton.IsEnabled = true;
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

        private void Drop()
        {
            _inventory.RemoveItem(_selectedItem, _dungeonMaster, _logManager);

            var mapConsoleResult = _dungeonMaster.GetCurrentMapConsole();
            if (_dungeonMaster.ModeMaster.Mode == GameMode.Dungeon
                && mapConsoleResult.HasValue)
            {
                var droppedItem = _dungeonMaster.ModeMaster.EntityFactory.CreateItem(_dungeonMaster.Player.Position, _selectedItem);
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

            _dropButton.IsEnabled = false;

            SetupSelectionButtons(buttons);
        }
    }
}

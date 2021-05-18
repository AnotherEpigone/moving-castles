using MovingCastles.Components;
using MovingCastles.Ui.Consoles;
using MovingCastles.Ui.Windows;
using SadConsole;

namespace MovingCastles.Ui
{
    public class MapModeMenuProvider : IMapModeMenuProvider
    {
        private InventoryConsole _inventory;

        public MapModeMenuProvider(
            DeathWindow death,
            PopupMenuWindow pop,
            SpellSelectionWindow spellSelect,
            CommandWindow command,
            JournalWindow journal)
        {
            Death = death;
            Pop = pop;
            SpellSelect = spellSelect;
            Command = command;
            Journal = journal;
        }

        public DeathWindow Death { get; }

        public PopupMenuWindow Pop { get; }

        public SpellSelectionWindow SpellSelect { get; }

        public CommandWindow Command { get; }

        public JournalWindow Journal { get; }

        public void HideInventoryPanel()
        {
            if (_inventory == null)
            {
                return;
            }

            _inventory.Hide();
            var mapConsole = (Console)((MainConsole)Global.CurrentScreen).MapConsole;
            mapConsole.IsFocused = true;
        }

        public void SetInventoryPanel(InventoryConsole inventoryConsole)
        {
            _inventory = inventoryConsole;
        }

        public void ShowInventoryPanel(IInventoryComponent inventory, IEquipmentComponent equipment)
        {
            if (_inventory == null)
            {
                return;
            }

            _inventory.Show(inventory, equipment, () => HideInventoryPanel());
            _inventory.IsFocused = true;
        }
    }
}

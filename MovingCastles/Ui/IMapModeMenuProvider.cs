using MovingCastles.Components;
using MovingCastles.Ui.Consoles;
using MovingCastles.Ui.Windows;

namespace MovingCastles.Ui
{
    public interface IMapModeMenuProvider
    {
        void SetInventoryPanel(InventoryConsole inventoryConsole);

        void ShowInventoryPanel(IInventoryComponent inventory);

        void HideInventoryPanel();

        DeathWindow Death { get; }

        PopupMenuWindow Pop { get; }

        SpellSelectionWindow SpellSelect { get; }

        CommandWindow Command { get; }

        JournalWindow Journal { get; }
    }
}
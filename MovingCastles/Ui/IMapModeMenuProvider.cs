using MovingCastles.Ui.Windows;

namespace MovingCastles.Ui
{
    public interface IMapModeMenuProvider
    {
        InventoryWindow Inventory { get; }

        DeathWindow Death { get; }
    }
}
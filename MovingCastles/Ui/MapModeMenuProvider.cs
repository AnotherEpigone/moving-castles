using MovingCastles.Ui.Windows;

namespace MovingCastles.Ui
{
    public class MapModeMenuProvider : IMapModeMenuProvider
    {
        public MapModeMenuProvider(
            InventoryWindow inventory,
            DeathWindow death,
            PopupMenuWindow pop)
        {
            Inventory = inventory;
            Death = death;
            Pop = pop;
        }

        public InventoryWindow Inventory { get; }

        public DeathWindow Death { get; }

        public PopupMenuWindow Pop { get; }
    }
}

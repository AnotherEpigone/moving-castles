using MovingCastles.Ui.Windows;

namespace MovingCastles.Ui
{
    public class MapModeMenuProvider : IMapModeMenuProvider
    {
        public MapModeMenuProvider(
            InventoryWindow inventory,
            DeathWindow death,
            PopupMenuWindow pop,
            SpellSelectionWindow spellSelect)
        {
            Inventory = inventory;
            Death = death;
            Pop = pop;
            SpellSelect = spellSelect;
        }

        public InventoryWindow Inventory { get; }

        public DeathWindow Death { get; }

        public PopupMenuWindow Pop { get; }

        public SpellSelectionWindow SpellSelect { get; }
    }
}

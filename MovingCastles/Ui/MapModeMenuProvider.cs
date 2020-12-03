using MovingCastles.Ui.Windows;

namespace MovingCastles.Ui
{
    public class MapModeMenuProvider : IMapModeMenuProvider
    {
        public MapModeMenuProvider(
            InventoryWindow inventory,
            DeathWindow death,
            PopupMenuWindow pop,
            SpellSelectionWindow spellSelect,
            CommandWindow command)
        {
            Inventory = inventory;
            Death = death;
            Pop = pop;
            SpellSelect = spellSelect;
            Command = command;
        }

        public InventoryWindow Inventory { get; }

        public DeathWindow Death { get; }

        public PopupMenuWindow Pop { get; }

        public SpellSelectionWindow SpellSelect { get; }

        public CommandWindow Command { get; }
    }
}

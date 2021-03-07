using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Journal;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Player
{
    public record WizardTemplate
    {
        public WizardTemplate()
            : this(
                "Vede",
                100,
                100,
                20,
                20,
                new List<ItemTemplate>(),
                new List<JournalEntry>())
        { }

        public WizardTemplate(
            string name,
            float health,
            float maxHealth,
            float endowment,
            float maxEndowment,
            List<ItemTemplate> items,
            List<JournalEntry> journalEntries)
            => (Name, Health, MaxHealth, Endowment, MaxEndowment, Items, JournalEntries)
                = (name, health, maxHealth, endowment, maxEndowment, items, journalEntries);

        public string Name { get; }
        public float Health { get; }
        public float MaxHealth { get; }
        public float Endowment { get; }
        public float MaxEndowment { get; }
        public List<ItemTemplate> Items { get; }
        public List<JournalEntry> JournalEntries { get; init; }
    }
}

using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Journal;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Player
{
    public record PlayerTemplate
    {
        public PlayerTemplate(
            string name,
            float health,
            float maxHealth,
            List<ItemTemplate> items,
            List<JournalEntry> journalEntries)
            => (Name, Health, MaxHealth, Items, JournalEntries)
                = (name, health, maxHealth, items, journalEntries);

        public string Name { get; }
        public float Health { get; }
        public float MaxHealth { get; }
        public List<ItemTemplate> Items { get; }
        public List<JournalEntry> JournalEntries { get; }

        public static PlayerTemplate CreateDefault() => new PlayerTemplate(
            "Vede",
            100,
            100,
            new List<ItemTemplate>(),
            new List<JournalEntry>());
    }
}

using MovingCastles.GameSystems.Time;
using MovingCastles.Text;

namespace MovingCastles.GameSystems.Journal.Entries
{
    public static class AlwardsTowerJournalEntries
    {
        public const string AlwardsTowerTopic = "Alward's Tower";

        public static JournalEntry Quest(McTimeSpan time) => new JournalEntry(AlwardsTowerTopic, "ENTRY_ALWARD_FIRSTENTRANCE", Story.Entry_AlwardsTower_Entrance, time);
        public static JournalEntry FirstEntrance(McTimeSpan time) => new JournalEntry(AlwardsTowerTopic, "ENTRY_ALWARD_QUEST", Story.Entry_AlwardsTower_Quest, time);
    }
}

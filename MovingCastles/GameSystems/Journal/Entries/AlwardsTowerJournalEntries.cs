﻿using MovingCastles.Text;

namespace MovingCastles.GameSystems.Journal.Entries
{
    public static class AlwardsTowerJournalEntries
    {
        public const string AlwardsTowerTopic = "Alward's Tower";

        public static JournalEntry FirstEntrance => new JournalEntry(AlwardsTowerTopic, "ENTRY_ALWARD_FIRSTENTRANCE", Story.Entry_AlwardsTower_Entrance);
    }
}
namespace MovingCastles.GameSystems.Journal
{
    public record JournalEntry
    {
        public JournalEntry(string topicId, string id, string message)
        {
            Id = id;
            Message = message;
            TopicId = topicId;
        }

        public string TopicId { get; }

        public string Id { get; }

        public string Message { get; }
    }
}

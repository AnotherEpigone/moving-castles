using MovingCastles.GameSystems.Time;

namespace MovingCastles.GameSystems.Journal
{
    public record JournalEntry
    {
        public JournalEntry(string topicId, string id, string message, McTimeSpan receivedTime)
        {
            Id = id;
            Message = message;
            TopicId = topicId;
            ReceivedTime = receivedTime;
        }

        public string TopicId { get; }

        public string Id { get; }

        public string Message { get; }

        public McTimeSpan ReceivedTime { get; }
    }
}

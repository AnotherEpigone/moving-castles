using System;

namespace MovingCastles.GameSystems.Time
{
    public interface ITimeMaster
    {
        McTimeSpan JourneyTime { get; }

        event EventHandler<McTimeSpan> TimeUpdated;

        void Enqueue(ITimeMasterNode node);

        ITimeMasterNode Next();
    }
}
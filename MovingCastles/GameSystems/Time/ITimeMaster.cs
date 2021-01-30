using System;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Time
{
    public interface ITimeMaster
    {
        McTimeSpan JourneyTime { get; }
        IEnumerable<ITimeMasterNode> Nodes { get; }

        event EventHandler<McTimeSpan> TimeUpdated;

        void Enqueue(ITimeMasterNode node);

        ITimeMasterNode Next();
    }
}
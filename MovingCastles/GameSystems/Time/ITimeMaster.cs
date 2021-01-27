using System;

namespace MovingCastles.GameSystems.Time
{
    public interface ITimeMaster
    {
        McTimeSpan JourneyTime { get; }
    }
}
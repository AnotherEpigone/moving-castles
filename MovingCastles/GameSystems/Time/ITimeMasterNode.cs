using System;

namespace MovingCastles.GameSystems.Time
{
    public interface ITimeMasterNode
    {
        long Time { get; }

        (Func<long, long> action, long nextTime) Run();
    }
}
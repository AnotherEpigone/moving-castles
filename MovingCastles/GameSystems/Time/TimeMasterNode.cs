using System;

namespace MovingCastles.GameSystems.Time
{
    public class TimeMasterNode : ITimeMasterNode
    {
        private readonly Func<long, long> _action;

        public TimeMasterNode(Func<long, long> action, long time)
        {
            _action = action;
            Time = time;
        }

        public long Time { get; }

        public (Func<long, long>, long) Run()
        {
            var nextTime = _action(Time);
            return (_action, nextTime);
        }
    }
}

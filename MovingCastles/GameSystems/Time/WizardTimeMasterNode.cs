using System;

namespace MovingCastles.GameSystems.Time
{
    public class WizardTimeMasterNode : ITimeMasterNode
    {
        public long Time { get; }

        public WizardTimeMasterNode(long time)
        {
            Time = time;
        }

        public (Func<long, long>, long) Run()
        {
            // no-op, player turn is handled specially
            return ((_) => _, -1);
        }
    }
}

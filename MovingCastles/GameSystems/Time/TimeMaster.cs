using System;

namespace MovingCastles.GameSystems.Time
{
    public class TimeMaster : ITimeMaster
    {
        private readonly McTimeSpan _journeyTime;

        public TimeMaster()
        {
           _journeyTime = new McTimeSpan(0);
        }

        public McTimeSpan JourneyTime => new McTimeSpan(_journeyTime.Seconds);
    }
}

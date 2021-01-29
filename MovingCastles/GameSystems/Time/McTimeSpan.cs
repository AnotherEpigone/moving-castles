namespace MovingCastles.GameSystems.Time
{
    public sealed class McTimeSpan
    {
        private const int DaysPerMonth = 33;
        private const int MonthsPerYear = 12;
        private const int SecondsPerDay = 86400;
        private const int CentisecondsPerSecond = 100;
        private const int SecondsPerMonth = SecondsPerDay * DaysPerMonth;
        private const int SecondsPerYear = SecondsPerDay * DaysPerMonth * MonthsPerYear;

        private long _centiseconds;

        public McTimeSpan(long centiseconds)
        {
            _centiseconds = centiseconds;
        }

        public int Year => (int)(Seconds / SecondsPerYear);
        public int Month => ((int)(Seconds % SecondsPerYear) / SecondsPerMonth) + 1;
        public int Day => ((int)(Seconds % SecondsPerMonth) / SecondsPerDay) + 1;
        public long Seconds => _centiseconds / CentisecondsPerSecond;
        public long Ticks => _centiseconds;

        public void SetTicks(long centiseconds)
        {
            _centiseconds = centiseconds;
        }
    }
}

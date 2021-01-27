namespace MovingCastles.GameSystems.Time
{
    public sealed class McTimeSpan
    {
        private const int DaysPerMonth = 33;
        private const int MonthsPerYear = 12;
        private const int SecondsPerDay = 86400;
        private const int SecondsPerMonth = SecondsPerDay * DaysPerMonth;
        private const int SecondsPerYear = SecondsPerDay * DaysPerMonth * MonthsPerYear;

        private long _seconds;

        public McTimeSpan(long seconds)
        {
            _seconds = seconds;
        }

        public int Year => (int)(_seconds / SecondsPerYear);
        public int Month => (SecondsPerMonth / (int)(_seconds % SecondsPerYear)) + 1;
        public int Day => (SecondsPerDay / (int)(_seconds % SecondsPerMonth)) + 1;
        public long Seconds => _seconds;

        public void AddSeconds(int seconds)
        {
            _seconds += seconds;
        }
    }
}

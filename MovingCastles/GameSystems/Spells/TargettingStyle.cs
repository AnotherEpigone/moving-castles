namespace MovingCastles.GameSystems.Spells
{
    public class TargettingStyle : ITargettingStyle
    {
        public TargettingStyle(
            bool offensive,
            TargetMode targetMode,
            int range,
            bool canMiss)
        {
            Offensive = offensive;
            TargetMode = targetMode;
            Range = range;
            CanMiss = canMiss;
        }

        public bool Offensive { get; }

        public int Range { get; }

        public bool CanMiss { get; }

        public TargetMode TargetMode { get; }
    }
}

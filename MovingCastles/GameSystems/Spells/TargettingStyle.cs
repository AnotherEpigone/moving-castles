namespace MovingCastles.GameSystems.Spells
{
    public class TargettingStyle : ITargettingStyle
    {
        public TargettingStyle(
            bool offensive,
            TargetMode targetMode,
            int range)
        {
            Offensive = offensive;
            TargetMode = targetMode;
            Range = range;
        }

        public bool Offensive { get; }

        public int Range { get; }

        public TargetMode TargetMode { get; }
    }
}

namespace MovingCastles.GameSystems.Spells
{
    public class TargettingStyle : ITargettingStyle
    {
        public TargettingStyle(bool offensive, TargetMode targetMode)
        {
            Offensive = offensive;
            TargetMode = targetMode;
        }

        public bool Offensive { get; }
        public TargetMode TargetMode { get; }
    }
}

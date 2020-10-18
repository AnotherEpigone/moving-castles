namespace MovingCastles.GameSystems.Spells
{
    public enum TargetMode
    {
        SingleTarget,
    }

    public interface ITargettingStyle
    {
        bool Offensive { get; }

        TargetMode TargetMode { get; }
    }
}

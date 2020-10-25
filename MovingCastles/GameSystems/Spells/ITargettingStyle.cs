namespace MovingCastles.GameSystems.Spells
{
    public enum TargetMode
    {
        SingleTarget,
        Projectile,
    }

    public interface ITargettingStyle
    {
        bool Offensive { get; }

        int Range { get; }

        TargetMode TargetMode { get; }
    }
}

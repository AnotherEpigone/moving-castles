namespace MovingCastles.GameSystems.Spells
{
    public enum TargetMode
    {
        SingleTarget,
        Projectile,
        Self,
    }

    public interface ITargettingStyle
    {
        bool CanMiss { get; }

        bool Offensive { get; }

        int Range { get; }

        TargetMode TargetMode { get; }
    }
}

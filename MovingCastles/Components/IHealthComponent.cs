namespace MovingCastles.Components
{
    public interface IHealthComponent
    {
        int Health { get; }
        int MaxHealth { get; }

        void ApplyDamage(int damage);
        void ApplyHealing(int healing);
    }
}
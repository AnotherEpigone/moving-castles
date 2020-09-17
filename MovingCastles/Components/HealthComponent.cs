using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace MovingCastles.Components
{
    public class HealthComponent : IGameObjectComponent, IHealthComponent
    {
        public HealthComponent(int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public IGameObject Parent { get; set; }

        public int Health { get; private set; }

        public int MaxHealth { get; }

        public void ApplyDamage(int damage)
        {
            Health -= damage;
        }

        public void ApplyHealing(int healing)
        {
            Health = System.Math.Min(MaxHealth, Health + healing);
        }
    }
}

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

        public float Health { get; private set; }

        public float MaxHealth { get; }

        public bool Dead => Health < 0.001;

        public void ApplyDamage(float damage)
        {
            Health = System.Math.Max(0, Health - damage);
        }

        public void ApplyHealing(float healing)
        {
            Health = System.Math.Min(MaxHealth, Health + healing);
        }
    }
}

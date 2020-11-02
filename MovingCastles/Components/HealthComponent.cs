using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components
{
    public class HealthComponent : IGameObjectComponent, IHealthComponent
    {
        private float _health;

        public HealthComponent(float maxHealth)
            : this(maxHealth, maxHealth, 1) { }

        public HealthComponent(float maxHealth, float health, float baseRegen)
        {
            MaxHealth = maxHealth;
            Health = health;
            BaseRegen = baseRegen;
        }

        /// <summary>
        /// e = previous health
        /// </summary>
        public event System.EventHandler<float> HealthChanged;

        public IGameObject Parent { get; set; }

        public float Health
        {
            get { return _health; }
            private set
            {
                if (value == _health)
                {
                    return;
                }

                var prevHealth = _health;
                _health = value;
                HealthChanged?.Invoke(this, prevHealth);
            }
        }

        public float MaxHealth { get; }

        public bool Dead => Health < 0.001;

        public float BaseRegen { get; }

        public void ApplyDamage(float damage, ILogManager logManager)
        {
            Health = System.Math.Max(0, Health - damage);
            if (Dead && Parent is McEntity mcParent)
            {
                logManager.EventLog($"{mcParent.ColoredName} was slain.");
                mcParent.Remove();
            }
        }

        public void ApplyHealing(float healing)
        {
            Health = System.Math.Min(MaxHealth, Health + healing);
        }

        public void ApplyBaseRegen()
        {
            ApplyHealing(BaseRegen);
        }
    }
}

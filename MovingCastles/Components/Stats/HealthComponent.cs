using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MovingCastles.Components.Stats
{
    public class HealthComponent : IGameObjectComponent, IHealthComponent, ISerializableComponent
    {
        private float _health;

        public HealthComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            MaxHealth = stateObj.MaxHealth;
            Health = stateObj.Health;
            BaseRegen = stateObj.BaseRegen;
        }

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
                logManager.CombatLog($"{mcParent.ColoredName} was slain.");
                logManager.StoryLog($"{mcParent.ColoredName} was slain.");
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

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(HealthComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                MaxHealth = MaxHealth,
                Health = Health,
                BaseRegen = BaseRegen,
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public float MaxHealth;
            [DataMember] public float Health;
            [DataMember] public float BaseRegen;
        }
    }
}

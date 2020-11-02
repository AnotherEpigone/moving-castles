using GoRogue.GameFramework.Components;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components
{
    public interface IHealthComponent : IGameObjectComponent
    {
        event System.EventHandler<float> HealthChanged;

        float Health { get; }
        float MaxHealth { get; }
        bool Dead { get; }
        float BaseRegen { get; }

        void ApplyDamage(float damage, ILogManager logManager);
        void ApplyHealing(float healing);
    }
}
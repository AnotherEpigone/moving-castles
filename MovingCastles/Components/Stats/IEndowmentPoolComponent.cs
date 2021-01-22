using GoRogue.GameFramework.Components;

namespace MovingCastles.Components.Stats
{
    public interface IEndowmentPoolComponent : IGameObjectComponent
    {
        event System.EventHandler<float> ValueChanged;

        float Value { get; }
        float MaxValue { get; }
        float BaseRegen { get; }

        void ApplyDrain(float damage);
        void ApplyRestore(float healing);
        void ApplyBaseRegen();
    }
}

using MovingCastles.Components.Serialization;

namespace MovingCastles.Components.Effects
{
    public interface IHealthRegenEffect : ISerializableComponent, IDescribableEffect
    {
        float Value { get; }
    }
}

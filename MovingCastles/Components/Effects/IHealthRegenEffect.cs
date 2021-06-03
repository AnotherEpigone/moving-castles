using MovingCastles.Components.Serialization;

namespace MovingCastles.Components.Effects
{
    public interface IHealthRegenEffect : ISerializableComponent
    {
        float Value { get; }
    }
}

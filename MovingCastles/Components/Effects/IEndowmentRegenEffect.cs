using MovingCastles.Components.Serialization;

namespace MovingCastles.Components.Effects
{
    public interface IEndowmentRegenEffect : ISerializableComponent, IDescribableEffect
    {
        float Value { get; }
    }
}

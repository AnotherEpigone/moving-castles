using MovingCastles.Components.Serialization;

namespace MovingCastles.Components.Effects
{
    public interface IEndowmentRegenEffect : ISerializableComponent
    {
        float Value { get; }
    }
}

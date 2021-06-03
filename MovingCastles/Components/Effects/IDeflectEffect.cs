using MovingCastles.Components.Serialization;

namespace MovingCastles.Components.Effects
{
    interface IDeflectEffect : ISerializableComponent, IDescribableEffect
    {
        int DeflectModifier { get; }
    }
}

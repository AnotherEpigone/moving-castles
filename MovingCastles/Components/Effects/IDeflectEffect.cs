using MovingCastles.Components.Serialization;

namespace MovingCastles.Components.Effects
{
    interface IDeflectEffect : ISerializableComponent
    {
        int DeflectModifier { get; }
    }
}

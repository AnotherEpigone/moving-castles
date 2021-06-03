using MovingCastles.Components.Serialization;

namespace MovingCastles.Components.Effects
{
    public interface IFovRangeEffect : ISerializableComponent, IDescribableEffect
    {
        int Modifier { get; }
    }
}

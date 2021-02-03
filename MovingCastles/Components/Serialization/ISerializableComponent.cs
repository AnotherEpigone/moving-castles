using GoRogue.GameFramework.Components;

namespace MovingCastles.Components.Serialization
{
    public interface ISerializableComponent : IGameObjectComponent
    {
        ComponentSerializable GetSerializable();
    }
}

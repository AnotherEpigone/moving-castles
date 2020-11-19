using GoRogue.GameFramework.Components;

namespace MovingCastles.Components.Serialization
{
    interface ISerializableComponent : IGameObjectComponent
    {
        ComponentSerializable GetSerializable();
    }
}

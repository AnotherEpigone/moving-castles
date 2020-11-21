using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components
{
    public class UseDoorComponent : IInteractTriggeredComponent, ISerializableComponent
    {
        public IGameObject Parent { get; set; }

        public void Interact(McEntity interactingEntity, ILogManager logManager)
        {
            if (Parent is not Door door)
            {
                return;
            }

            door.Toggle(interactingEntity.Name, logManager);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(UseDoorComponent),
        };
    }
}

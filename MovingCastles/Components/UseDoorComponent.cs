using GoRogue.GameFramework;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;

namespace MovingCastles.Components
{
    public class UseDoorComponent : IInteractTriggeredComponent
    {
        public IGameObject Parent { get; set; }

        public void Interact(McEntity interactingEntity)
        {
            if (!(Parent is Door door))
            {
                return;
            }

            door.Toggle();
        }
    }
}

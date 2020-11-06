using GoRogue.GameFramework.Components;
using MovingCastles.Entities;

namespace MovingCastles.Components.Triggers
{
    public interface IInteractTriggeredComponent : IGameObjectComponent
    {
        void Interact(McEntity interactingEntity);
    }
}

using GoRogue.GameFramework.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components.Triggers
{
    public interface IInteractTriggeredComponent : IGameObjectComponent
    {
        void Interact(McEntity interactingEntity, ILogManager logManager, IDungeonMaster dungeonMaster);
    }
}

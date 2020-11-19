using GoRogue.GameFramework.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components.Triggers
{
    public interface IStepTriggeredComponent : IGameObjectComponent
    {
        void OnStep(McEntity steppingEntity, ILogManager logManager);
    }
}

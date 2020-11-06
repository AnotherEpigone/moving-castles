using GoRogue.GameFramework.Components;
using MovingCastles.Entities;

namespace MovingCastles.Components.Triggers
{
    public interface IStepTriggeredComponent : IGameObjectComponent
    {
        void OnStep(McEntity steppingEntity);
    }
}

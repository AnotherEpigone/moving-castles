using GoRogue.GameFramework.Components;
using MovingCastles.Entities;

namespace MovingCastles.Components
{
    public interface IStepTriggeredComponent : IGameObjectComponent
    {
        public void OnStep(McEntity steppingEntity);
    }
}

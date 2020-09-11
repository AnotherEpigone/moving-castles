using GoRogue;
using GoRogue.GameFramework.Components;

namespace MovingCastles.Components
{
    public interface IStepTriggeredComponent : IGameObjectComponent
    {
        public void OnStep(IHasComponents steppingEntity);
    }
}

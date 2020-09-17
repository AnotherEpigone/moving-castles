using GoRogue;
using GoRogue.GameFramework.Components;
using SadConsole;

namespace MovingCastles.Components
{
    public interface IStepTriggeredComponent : IGameObjectComponent
    {
        public void OnStep(BasicEntity steppingEntity);
    }
}

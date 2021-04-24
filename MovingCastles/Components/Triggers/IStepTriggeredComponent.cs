using GoRogue.GameFramework.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using Troschuetz.Random;

namespace MovingCastles.Components.Triggers
{
    public interface IStepTriggeredComponent : IGameObjectComponent
    {
        void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster gameManager, IGenerator rng);
    }
}

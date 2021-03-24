using GoRogue.GameFramework.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components.Triggers
{
    public interface IBumpTriggeredComponent : IGameObjectComponent
    {
        void Bump(McEntity bumpingEntity, ILogManager logManager, IDungeonMaster dungeonMaster);
    }
}

using GoRogue.GameFramework.Components;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;

namespace MovingCastles.Components.AiComponents
{
    public interface IAiComponent : IGameObjectComponent
    {
        bool Run(DungeonMap map, ILogManager logManager);
    }
}

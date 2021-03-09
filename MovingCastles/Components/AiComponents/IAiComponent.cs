using GoRogue.GameFramework.Components;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using Troschuetz.Random;

namespace MovingCastles.Components.AiComponents
{
    public interface IAiComponent : IGameObjectComponent
    {
        (bool success, int ticks) Run(McMap map, IGenerator rng, ILogManager logManager);
    }
}

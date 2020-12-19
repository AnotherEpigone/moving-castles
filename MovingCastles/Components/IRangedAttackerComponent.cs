using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using Troschuetz.Random;

namespace MovingCastles.Components
{
    public interface IRangedAttackerComponent
    {
        bool TryAttack(DungeonMap map, IGenerator rng, ILogManager logManager);
    }
}
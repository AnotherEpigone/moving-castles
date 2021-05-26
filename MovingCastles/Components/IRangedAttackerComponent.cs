using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using Troschuetz.Random;

namespace MovingCastles.Components
{
    public interface IRangedAttackerComponent
    {
        bool TryAttack(McMap map, IGenerator rng, IDungeonMaster dungeonMaster, ILogManager logManager);
    }
}
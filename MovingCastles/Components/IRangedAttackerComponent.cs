using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;

namespace MovingCastles.Components
{
    public interface IRangedAttackerComponent
    {
        bool TryAttack(DungeonMap map, ILogManager logManager);
    }
}
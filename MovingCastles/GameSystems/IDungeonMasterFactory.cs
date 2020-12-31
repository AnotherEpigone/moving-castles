using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;

namespace MovingCastles.GameSystems
{
    public interface IDungeonMasterFactory
    {
        IDungeonMaster Create(PlayerTemplate player, Level level, Structure structure);
    }
}
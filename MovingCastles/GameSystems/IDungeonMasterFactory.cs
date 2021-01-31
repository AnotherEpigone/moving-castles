using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Time;

namespace MovingCastles.GameSystems
{
    public interface IDungeonMasterFactory
    {
        IDungeonMaster Create(PlayerTemplate player, Level level, Structure structure);

        IDungeonMaster Create(PlayerTemplate player, Level level, Structure structure, ITimeMaster timeMaster);
    }
}
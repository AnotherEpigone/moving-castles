using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Time;

namespace MovingCastles.GameSystems
{
    public interface IDungeonMasterFactory
    {
        IDungeonMaster Create(Wizard player, Level level, Structure structure);

        IDungeonMaster Create(Wizard player, Level level, Structure structure, ITimeMaster timeMaster);
    }
}
using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Time;

namespace MovingCastles.GameSystems
{
    public interface IDungeonMasterFactory
    {
        IDungeonMaster Create(
            Wizard player,
            Level level,
            Structure structure,
            IGameModeMaster gameModeMaster,
            IStructureFactory structureFactory);

        IDungeonMaster Create(
            Wizard player,
            Level level,
            Structure structure,
            IGameModeMaster gameModeMaster,
            IStructureFactory structureFactory,
            ITimeMaster timeMaster);
    }
}
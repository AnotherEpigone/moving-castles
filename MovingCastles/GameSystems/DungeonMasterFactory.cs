using MovingCastles.Entities;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Time;

namespace MovingCastles.GameSystems
{
    public class DungeonMasterFactory : IDungeonMasterFactory
    {
        public IDungeonMaster Create(
            Wizard player,
            Level level,
            Structure structure,
            IGameModeMaster gameModeMaster,
            IStructureFactory structureFactory,
            ITimeMaster timeMaster)
        {
            var levelMaster = new LevelMaster(structureFactory, gameModeMaster)
            {
                Level = level,
                Structure = structure,
            };

            var factionMaster = new FactionMaster();

            return new DungeonMaster(
                player,
                levelMaster,
                factionMaster,
                timeMaster,
                gameModeMaster);
        }

        public IDungeonMaster Create(
            Wizard player,
            Level level,
            Structure structure,
            IGameModeMaster gameModeMaster,
            IStructureFactory structureFactory)
            => Create(
                player,
                level,
                structure,
                gameModeMaster,
                structureFactory,
                new TimeMaster());
    }
}

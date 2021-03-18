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
            IEntityFactory entityFactory,
            IStructureFactory structureFactory,
            ITimeMaster timeMaster)
        {
            var levelMaster = new LevelMaster(structureFactory, entityFactory)
            {
                Level = level,
                Structure = structure,
            };

            var factionMaster = new FactionMaster();

            return new DungeonMaster(
                player,
                levelMaster,
                factionMaster,
                timeMaster);
        }

        public IDungeonMaster Create(
            Wizard player,
            Level level,
            Structure structure,
            IEntityFactory entityFactory,
            IStructureFactory structureFactory)
            => Create(
                player,
                level,
                structure,
                entityFactory,
                structureFactory,
                new TimeMaster());
    }
}

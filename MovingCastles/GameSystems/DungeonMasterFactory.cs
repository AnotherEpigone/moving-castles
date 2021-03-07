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
            ITimeMaster timeMaster)
        {
            var levelMaster = new LevelMaster()
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

        public IDungeonMaster Create(Wizard player, Level level, Structure structure)
            => Create(player, level, structure, new TimeMaster());
    }
}

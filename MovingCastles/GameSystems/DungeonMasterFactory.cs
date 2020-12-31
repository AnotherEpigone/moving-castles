using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;

namespace MovingCastles.GameSystems
{
    public class DungeonMasterFactory : IDungeonMasterFactory
    {
        public IDungeonMaster Create(
            PlayerTemplate player,
            Level level,
            Structure structure)
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
                factionMaster);
        }
    }
}

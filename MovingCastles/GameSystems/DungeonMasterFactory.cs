using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;

namespace MovingCastles.GameSystems
{
    public class DungeonMasterFactory : IDungeonMasterFactory
    {
        public IDungeonMaster Create(
            PlayerInfo player,
            Level level,
            Structure structure)
        {
            var levelMaster = new LevelMaster()
            {
                Level = level,
                Structure = structure,
            };

            return new DungeonMaster(player, levelMaster);
        }
    }
}

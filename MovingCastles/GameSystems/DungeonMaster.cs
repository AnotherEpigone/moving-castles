using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;

namespace MovingCastles.GameSystems
{
    public class DungeonMaster : IDungeonMaster
    {
        public DungeonMaster(PlayerInfo player)
        {
            Player = player;
        }

        public PlayerInfo Player { get; }

        public Level Level { get; set; }
    }
}

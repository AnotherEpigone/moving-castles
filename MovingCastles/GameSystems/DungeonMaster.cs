using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;

namespace MovingCastles.GameSystems
{
    public class DungeonMaster : IDungeonMaster
    {
        public DungeonMaster(
            PlayerInfo player,
            ILevelMaster levelMaster,
            IFactionMaster factionMaster)
        {
            Player = player;
            LevelMaster = levelMaster;
            FactionMaster = factionMaster;
        }

        public PlayerInfo Player { get; }

        public ILevelMaster LevelMaster { get; }

        public IFactionMaster FactionMaster { get; }
    }
}

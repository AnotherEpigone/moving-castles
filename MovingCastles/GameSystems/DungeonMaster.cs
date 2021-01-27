using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Time;

namespace MovingCastles.GameSystems
{
    public class DungeonMaster : IDungeonMaster
    {
        public DungeonMaster(
            PlayerTemplate player,
            ILevelMaster levelMaster,
            IFactionMaster factionMaster,
            ITimeMaster timeMaster)
        {
            Player = player;
            LevelMaster = levelMaster;
            FactionMaster = factionMaster;
            TimeMaster = timeMaster;
        }

        public PlayerTemplate Player { get; }
        public ILevelMaster LevelMaster { get; }
        public IFactionMaster FactionMaster { get; }
        public ITimeMaster TimeMaster { get; }
    }
}

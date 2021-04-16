using MovingCastles.Entities;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Time;
using MovingCastles.Ui.Windows;

namespace MovingCastles.GameSystems
{
    public class DungeonMaster : IDungeonMaster
    {
        public DungeonMaster(
            Wizard player,
            ILevelMaster levelMaster,
            IFactionMaster factionMaster,
            ITimeMaster timeMaster,
            IGameModeMaster modeMaster,
            StoryActionWindow storyActionWindow)
        {
            Player = player;
            LevelMaster = levelMaster;
            FactionMaster = factionMaster;
            TimeMaster = timeMaster;
            ModeMaster = modeMaster;
            StoryActionWindow = storyActionWindow;
        }

        public Wizard Player { get; }
        public IGameModeMaster ModeMaster { get; }
        public ILevelMaster LevelMaster { get; }
        public IFactionMaster FactionMaster { get; }
        public ITimeMaster TimeMaster { get; }
        public StoryActionWindow StoryActionWindow { get; }
    }
}

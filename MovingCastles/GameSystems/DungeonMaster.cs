using MovingCastles.Entities;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Time;
using MovingCastles.Ui.Consoles;
using Optional;

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
            IScenarioMaster scenarioMaster)
        {
            Player = player;
            LevelMaster = levelMaster;
            FactionMaster = factionMaster;
            TimeMaster = timeMaster;
            ModeMaster = modeMaster;
            ScenarioMaster = scenarioMaster;
        }

        public Wizard Player { get; }
        public IGameModeMaster ModeMaster { get; }
        public ILevelMaster LevelMaster { get; }
        public IFactionMaster FactionMaster { get; }
        public ITimeMaster TimeMaster { get; }
        public IScenarioMaster ScenarioMaster { get; }

        public Option<DungeonMapConsole> GetCurrentMapConsole()
        {
            if (!(SadConsole.Global.CurrentScreen is MainConsole main)
                || !(main.MapConsole is DungeonMapConsole mapConsole))
            {
                return Option.None<DungeonMapConsole>();
            }

            return Option.Some(mapConsole);
        }
    }
}

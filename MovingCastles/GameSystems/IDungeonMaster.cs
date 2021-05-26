using MovingCastles.Entities;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Time;
using MovingCastles.Ui.Consoles;
using Optional;

namespace MovingCastles.GameSystems
{
    public interface IDungeonMaster
    {
        Wizard Player { get; }

        IGameModeMaster ModeMaster { get; }

        ILevelMaster LevelMaster { get; }

        IFactionMaster FactionMaster { get; }

        ITimeMaster TimeMaster { get; }

        IScenarioMaster ScenarioMaster { get; }
        IHitMan HitMan { get; }

        Option<DungeonMapConsole> GetCurrentMapConsole();
    }
}
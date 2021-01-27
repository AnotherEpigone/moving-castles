using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Time;

namespace MovingCastles.GameSystems
{
    public interface IDungeonMaster
    {
        PlayerTemplate Player { get; }

        ILevelMaster LevelMaster { get; }

        IFactionMaster FactionMaster { get; }

        ITimeMaster TimeMaster { get; }
    }
}
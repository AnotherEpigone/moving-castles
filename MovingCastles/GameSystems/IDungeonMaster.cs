using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;

namespace MovingCastles.GameSystems
{
    public interface IDungeonMaster
    {
        Level Level { get; set; }
        PlayerInfo Player { get; }
        Structure Structure { get; set; }
    }
}
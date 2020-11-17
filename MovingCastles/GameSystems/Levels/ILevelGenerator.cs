using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Saving;

namespace MovingCastles.GameSystems.Levels
{
    public interface ILevelGenerator
    {
        string Id { get; }

        Level Generate(int seed, PlayerInfo playerInfo);

        Level Generate(Save save);
    }
}

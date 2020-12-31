using GoRogue;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Saving;
using MovingCastles.Serialization.Map;

namespace MovingCastles.GameSystems.Levels
{
    public interface ILevelGenerator
    {
        string Id { get; }

        Level Generate(int seed, string id, PlayerTemplate playerInfo, SpawnConditions playerSpawnConditions);

        Level Generate(Save save);

        Level Generate(MapState mapState, PlayerTemplate playerInfo, SpawnConditions playerSpawnConditions);

        Level Generate(int seed, string id, PlayerTemplate playerInfo, Coord playerSpawnPosition);
    }
}

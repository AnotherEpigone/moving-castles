using MovingCastles.Entities;
using MovingCastles.GameSystems.Saving;
using MovingCastles.Serialization.Map;

namespace MovingCastles.GameSystems.Levels
{
    public interface ILevelGenerator
    {
        string Id { get; }

        /// <summary>
        /// New level from scratch
        /// </summary>
        Level Generate(int seed, string id, Wizard player, SpawnConditions playerSpawnConditions);

        /// <summary>
        /// Load from Save
        /// </summary>
        Level Generate(Save save);

        /// <summary>
        /// Return to previously generated level
        /// </summary>
        Level Generate(MapState mapState, Wizard player, SpawnConditions playerSpawnConditions);
    }
}

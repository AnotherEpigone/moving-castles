using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Saving;
using MovingCastles.Serialization.Map;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Levels
{
    /// <summary>
    /// A set of Levels that are logically linked
    /// </summary>
    public class Structure
    {
        public const string StructureId_MapgenDemo = "STRUCTURE_MAPGEN_DEMO";
        public const string StructureId_AlwardsTower = "STRUCTURE_ALWARDS_TOWER";

        private readonly Dictionary<string, Level> _generatedLevels;
        private readonly Dictionary<string, MapState> _serializedLevels;

        public Structure(string id)
        {
            _generatedLevels = new Dictionary<string, Level>();
            _serializedLevels = new Dictionary<string, MapState>();
            Generators = new Dictionary<string, ILevelGenerator>();

            Id = id;
        }

        public string Id { get; init; }

        public Dictionary<string, ILevelGenerator> Generators { get; }

        public Level GetLevel(string id, PlayerTemplate playerInfo, SpawnConditions playerSpawnConditions)
        {
            if (_generatedLevels.TryGetValue(id, out Level level))
            {
                return level;
            }

            var generator = Generators[id];
            if (_serializedLevels.TryGetValue(id, out var mapState))
            {
                level = generator.Generate(mapState, playerInfo, playerSpawnConditions);
            }
            else
            {
                level = generator.Generate(McRandom.GetSeed(), id, playerInfo, playerSpawnConditions);
            }

            _generatedLevels.Add(id, level);
            return level;
        }

        public Level GetLevel(Save save)
        {
            var generator = Generators[save.MapState.Id];

            var level = generator.Generate(save);

            _generatedLevels.Add(save.MapState.Id, level);
            return level;
        }
    }
}

using MovingCastles.Entities;
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

        public Structure(string id)
        {
            GeneratedLevels = new Dictionary<string, Level>();
            SerializedLevels = new Dictionary<string, MapState>();
            Generators = new Dictionary<string, ILevelGenerator>();

            Id = id;
        }

        public string Id { get; init; }

        public Dictionary<string, ILevelGenerator> Generators { get; }

        public Dictionary<string, Level> GeneratedLevels { get; }

        public Dictionary<string, MapState> SerializedLevels { get; }

        public Level GetLevel(string id, Wizard player, SpawnConditions playerSpawnConditions)
        {
            if (GeneratedLevels.TryGetValue(id, out Level level))
            {
                return level;
            }

            var generator = Generators[id];
            if (SerializedLevels.TryGetValue(id, out var mapState))
            {
                level = generator.Generate(mapState, player, playerSpawnConditions);
            }
            else
            {
                level = generator.Generate(McRandom.GetSeed(), id, player, playerSpawnConditions);
            }

            GeneratedLevels.Add(id, level);
            return level;
        }

        public Level GetLevel(Save save)
        {
            var generator = Generators[save.MapState.Id];

            var level = generator.Generate(save);

            GeneratedLevels.Add(save.MapState.Id, level);

            foreach (var serializedLevel in save.KnownMaps)
            {
                if (!GeneratedLevels.ContainsKey(serializedLevel.Id) && !SerializedLevels.ContainsKey(serializedLevel.Id))
                {
                    SerializedLevels.Add(serializedLevel.Id, serializedLevel);
                }
            }

            return level;
        }
    }
}

using MovingCastles.Entities;
using MovingCastles.GameSystems.Player;
using System;
using Troschuetz.Random;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public class MapgenDemoLevelGenerator : LevelGenerator
    {
        protected MapgenDemoLevelGenerator(IEntityFactory entityFactory)
            : base(entityFactory) { }

        public override string Id { get; } = "GENERATOR_MAPGENDEMO";

        public override Level Generate(int seed, string id, PlayerTemplate playerInfo, SpawnConditions playerSpawnConditions)
        {
            throw new NotImplementedException();
        }

        protected override Level GenerateTerrain(IGenerator rng, int seed, string id, int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}

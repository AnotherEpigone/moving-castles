using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using MovingCastles.Components.Levels;
using MovingCastles.Entities;
using MovingCastles.Maps;
using System.Collections.Generic;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public class SaraniHighlandsLevelGenerator : LevelGenerator
    {
        public SaraniHighlandsLevelGenerator(IGameModeMaster gameModeMaster)
            : base(gameModeMaster) { }

        public override string Id { get; } = "GENERATOR_SARANI_HIGHLANDS";

        public override Level Generate(int seed, string id, Wizard player, SpawnConditions playerSpawnConditions)
        {
            var rng = new StandardGenerator(seed);
            var level = Generate(seed, id, rng);

            // spawn player
            player.Position = SpawnHelper.GetSpawnPosition(level, playerSpawnConditions, rng);
            level.Map.AddEntity(player);

            return level;
        }

        protected override (Level, LevelGenerationMetadata) GenerateTerrain(IGenerator rng, int seed, string id, int width, int height)
        {
            var map = new McMap(width, height);
            var terrain = new ArrayMap<bool>(width, height);

            QuickGenerators.GenerateRectangleMap(terrain);
            map.ApplyTerrainOverlay(terrain, TerrainSpawning.SpawnMountainTerrain);

            var level = new Level(id, "Sarani Desert Highlands", seed, new List<Room>(), new List<Coord>(), map);
            var meta = new LevelGenerationMetadata();

            return (level, meta);
        }

        private Level Generate(int seed, string id, IGenerator rng)
        {
            var (level, _) = GenerateTerrain(rng, seed, id, 20, 20);
            var map = level.Map;

            // spawn doors
            foreach (var door in level.Doors)
            {
                map.AddEntity(GameModeMaster.EntityFactory.CreateDoor(door));
            }

            var spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
            var tower = GameModeMaster.EntityFactory.CreateDoodad(spawnPosition, CastleModeDoodadAtlas.AlwardsTower);
            tower.AddGoRogueComponent(new ChangeStructureComponent(Structure.StructureId_AlwardsTower, LevelId.AlwardsTower1, new SpawnConditions(Spawn.Default, 0)));
            map.AddEntity(tower);

            return level;
        }
    }
}

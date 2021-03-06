﻿using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using MovingCastles.Entities;
using MovingCastles.Maps;
using MovingCastles.Maps.Generation;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public class MapgenDemoLevelGenerator : LevelGenerator
    {
        private const string DungeonAreaKey = "MAPAREA_DUNGEON";

        public MapgenDemoLevelGenerator(IGameModeMaster gameModeMaster)
            : base(gameModeMaster) { }

        public override string Id { get; } = "GENERATOR_MAPGENDEMO";

        public override Level Generate(int seed, string id, Wizard player, SpawnConditions playerSpawnConditions)
        {
            var rng = new StandardGenerator(seed);
            var (level, meta) = GenerateTerrain(rng, seed, LevelId.MapgenTest, 100, 60);

            foreach (var door in level.Doors)
            {
                level.Map.AddEntity(GameModeMaster.EntityFactory.CreateDoor(door));
            }

            // spawn a trapdoor
            var roomDungeonRect = meta.Areas[DungeonAreaKey];       
            var spawnPosition = level.Map.WalkabilityView.RandomPosition((pos, walkable) => walkable && roomDungeonRect.Contains(pos));
            var trapdoor = GameModeMaster.EntityFactory.CreateDoodad(spawnPosition, DungeonModeDoodadAtlas.Trapdoor);
            level.Map.AddEntity(trapdoor);

            // spawn player
            //spawnPosition = spawnPosition = map.WalkabilityView.RandomPosition((pos, walkable) => walkable && roomDungeonRect.Contains(pos));
            player.Position = new Coord(24, 2);
            level.Map.AddEntity(player);

            // No FOV by default
            level.Map.FovVisibilityHandler.Disable();

            return level;
        }

        protected override (Level, LevelGenerationMetadata) GenerateTerrain(IGenerator rng, int seed, string id, int width, int height)
        {
            var map = new McMap(width, height);

            // blank canvas
            var emptyMapTerrain = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateRectangleMap(emptyMapTerrain);
            map.ApplyTerrainOverlay(emptyMapTerrain, TerrainSpawning.SpawnOutdoorTerrain);

            // add a 20x20 maze with an entrance
            var mazeTerrain = new ArrayMap<bool>(20, 20);
            new BorderlessMazeGenerator().Generate(rng, mazeTerrain);
            map.ApplyTerrainOverlay(mazeTerrain, new Coord(2, 2), TerrainSpawning.SpawnDungeonTerrain);

            // add a 20x20 forest, hopefully with a path through it (a path! a path!)
            var forestPathTerrain = new ArrayMap<bool>(20, 20);
            new TwoEntranceMazeGenerator().Generate(rng, forestPathTerrain);
            map.ApplyTerrainOverlay(forestPathTerrain, new Coord(25, 2), TerrainSpawning.SpawnOutdoorTerrain);

            // add a 30x30 dungeon with rooms
            var roomDungeonTerrain = new ArrayMap<bool>(30, 30);
            var roomLocations = new RoomGenerator(rng).FillRooms(roomDungeonTerrain, 10, 3, 3, new List<Rectangle>());
            var roomDungeonOffset = new Coord(2, 25);
            var doorGen = new DoorGenerator(rng);
            var doorsRound1 = doorGen.GenerateRandom(roomDungeonTerrain, roomLocations);
            map.ApplyTerrainOverlay(roomDungeonTerrain, roomDungeonOffset, TerrainSpawning.SpawnDungeonTerrain);

            // extra doors to ensure walkability
            var doorsRound2 = doorGen.GenerateForWalkability(map, roomDungeonTerrain, roomDungeonOffset, roomLocations);
            map.ApplyTerrainOverlay(roomDungeonTerrain, roomDungeonOffset, TerrainSpawning.SpawnDungeonTerrain);

            var globalDoors = doorsRound1
                .Concat(doorsRound2)
                .Select(d => d + roomDungeonOffset)
                .ToList();

            var rooms = roomLocations.Select(l => new Room(l, RoomType.None)).ToList();

            var level = new Level(
                id,
                "Mapgen test area",
                seed,
                rooms,
                globalDoors,
                map);

            var roomDungeonRect = new Rectangle(roomDungeonOffset.X, roomDungeonOffset.Y, roomDungeonTerrain.Width, roomDungeonTerrain.Height);
            var meta = new LevelGenerationMetadata();
            meta.Areas.Add(DungeonAreaKey, roomDungeonRect);

            return (level, meta);
        }
    }
}

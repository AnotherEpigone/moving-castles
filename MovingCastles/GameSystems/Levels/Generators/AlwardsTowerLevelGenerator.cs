﻿using GoRogue.MapViews;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Saving;
using MovingCastles.Maps;
using MovingCastles.Maps.Generation;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random.Generators;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public class AlwardsTowerLevelGenerator : ILevelGenerator
    {
        private readonly IEntityFactory _entityFactory;

        public AlwardsTowerLevelGenerator(IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
        }

        public string Id { get; } = "GENERATOR_ALWARDS_TOWER";

        public Level Generate(int seed, PlayerInfo playerInfo)
        {
            var level = GenerateTerrainWithDoorLocations(seed);
            var map = level.Map;

            // spawn doors
            foreach (var door in level.Doors)
            {
                map.AddEntity(_entityFactory.CreateDoor(door));
            }

            var rng = new StandardGenerator(seed);

            // spawn a trapdoor
            var spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
            var trapdoor = _entityFactory.CreateDoodad(spawnPosition, DoodadAtlas.Trapdoor);
            map.AddEntity(trapdoor);

            // spawn player
            spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
            var player = _entityFactory.CreatePlayer(spawnPosition, playerInfo);
            map.AddEntity(player);

            return level;
        }

        public Level Generate(Save save)
        {
            var level = GenerateTerrainWithDoorLocations(save.Seed);
            foreach (var entity in save.Entities)
            {
                level.Map.AddEntity(entity);
            }

            foreach (var door in save.Doors)
            {
                level.Map.AddEntity(door);
            }

            level.Map.AddEntity(save.Wizard);

            return level;
        }

        private Level GenerateTerrainWithDoorLocations(int seed)
        {
            var rng = new StandardGenerator(seed);
            var map = new DungeonMap(30, 30);
            var terrain = new ArrayMap<bool>(30, 30);

            var rooms = new RoomFiller(rng).Generate(terrain, 12, 2, 2);

            var doorGen = new DoorGenerator(rng);
            var doorsRound1 = doorGen.GenerateRandom(terrain, rooms);
            map.ApplyTerrainOverlay(terrain, MapFactory.SpawnDungeonTerrain);

            var doorsRound2 = doorGen.GenerateForWalkability(map, terrain, rooms);
            map.ApplyTerrainOverlay(terrain, MapFactory.SpawnDungeonTerrain);

            return new Level(seed, rooms, doorsRound1.Concat(doorsRound2).ToList(), map);
        }
    }
}

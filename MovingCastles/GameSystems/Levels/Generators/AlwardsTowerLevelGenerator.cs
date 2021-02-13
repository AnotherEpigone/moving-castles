﻿using GoRogue;
using GoRogue.MapViews;
using MovingCastles.Components.Levels;
using MovingCastles.Components.StoryComponents;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Player;
using MovingCastles.Maps;
using MovingCastles.Maps.Generation;
using MovingCastles.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public sealed class AlwardsTowerLevelGenerator : LevelGenerator
    {
        private static readonly List<ItemTemplate> _floorItems = new List<ItemTemplate>()
        {
            ItemAtlas.EtheriumShard,
            ItemAtlas.SteelLongsword,
        };

        public AlwardsTowerLevelGenerator(IEntityFactory entityFactory)
            : base(entityFactory) { }

        public override string Id { get; } = "GENERATOR_ALWARDS_TOWER";

        public override Level Generate(int seed, string id, PlayerTemplate playerInfo, SpawnConditions playerSpawnConditions)
        {
            var rng = new StandardGenerator(seed);
            var level = Generate(seed, id, rng);

            // spawn player
            var spawnPosition = SpawnHelper.GetSpawnPosition(level, playerSpawnConditions, rng);
            var player = EntityFactory.CreatePlayer(spawnPosition, playerInfo);
            level.Map.AddEntity(player);

            return level;
        }

        protected override Level GenerateTerrain(IGenerator rng, int seed, string id, int width, int height)
        {
            var map = new DungeonMap(width, height);
            var terrain = new ArrayMap<bool>(width, height);

            var rooms = new RoomFiller(rng).Generate(terrain, 12, 2, 2);

            var doorGen = new DoorGenerator(rng);
            var doorsRound1 = doorGen.GenerateRandom(terrain, rooms);
            map.ApplyTerrainOverlay(terrain, MapFactory.SpawnDungeonTerrain);

            var doorsRound2 = doorGen.GenerateForWalkability(map, terrain, rooms);
            map.ApplyTerrainOverlay(terrain, MapFactory.SpawnDungeonTerrain);

            var name = id switch
            {
                LevelId.AlwardsTower1 => "Old Alward's Tower, floor 1",
                LevelId.AlwardsTower2 => "Old Alward's Tower, floor 2",
                _ => throw new ArgumentException($"Invalid level id {nameof(id)} for generator {nameof(AlwardsTowerLevelGenerator)}"),
            };

            return new Level(id, name, seed, rooms, doorsRound1.Concat(doorsRound2).ToList(), map);
        }

        private Level Generate(int seed, string id, IGenerator rng)
        {
            var level = GenerateTerrain(rng, seed, id, 30, 30);
            var map = level.Map;

            // spawn doors
            foreach (var door in level.Doors)
            {
                map.AddEntity(EntityFactory.CreateDoor(door));
            }

            // spawn doodads
            Coord spawnPosition;
            if (id == LevelId.AlwardsTower1)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var trapdoor = EntityFactory.CreateDoodad(spawnPosition, DoodadAtlas.Trapdoor);
                trapdoor.AddGoRogueComponent(new StoryMessageBoxComponent(nameof(Story.AlwardsTower_TrapdoorStep), true));
                map.AddEntity(trapdoor);

                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var stairsUp = EntityFactory.CreateDoodad(spawnPosition, DoodadAtlas.StaircaseUp);
                stairsUp.AddGoRogueComponent(new ChangeLevelComponent(LevelId.AlwardsTower2, new SpawnConditions(Spawn.Stairdown, 0)));
                map.AddEntity(stairsUp);
            }

            if (id == LevelId.AlwardsTower2)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var etheriumCore = EntityFactory.CreateDoodad(spawnPosition, DoodadAtlas.EtheriumCoreWithStand);
                map.AddEntity(etheriumCore);

                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var stairDown = EntityFactory.CreateDoodad(spawnPosition, DoodadAtlas.StaircaseDown);
                stairDown.AddGoRogueComponent(new ChangeLevelComponent(LevelId.AlwardsTower1, new SpawnConditions(Spawn.StairUp, 0)));
                map.AddEntity(stairDown);
            }

            // Spawn enemies
            var allTheActors = ActorAtlas.ActorsById.Values.ToList();
            for (int i = 0; i < 10; i++)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true);

                var enemy = EntityFactory.CreateActor(spawnPosition, allTheActors.RandomItem());
                map.AddEntity(enemy);
            }

            // Spawn items
            for (int i = 0; i < 10; i++)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true);

                var item = EntityFactory.CreateItem(spawnPosition, _floorItems.RandomItem());

                map.AddEntity(item);
            }

            return level;
        }
    }
}

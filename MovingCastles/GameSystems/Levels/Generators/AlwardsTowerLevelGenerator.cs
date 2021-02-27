﻿using GoRogue;
using GoRogue.MapViews;
using MovingCastles.Components.Levels;
using MovingCastles.Components.StoryComponents;
using MovingCastles.Entities;
using MovingCastles.Extensions;
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

        protected override (Level, LevelGenerationMetadata) GenerateTerrain(IGenerator rng, int seed, string id, int width, int height)
        {
            var map = new DungeonMap(width, height);
            var terrain = new ArrayMap<bool>(width, height);

            var roomFiller = new RoomGenerator(rng);

            var staticRooms = new List<Room>();
            if (id == LevelId.AlwardsTower1)
            {
                staticRooms.Add(new Room(roomFiller.PlaceRoom(terrain, 3, 3, staticRooms.Select(r => r.Location)), RoomType.Stairwell));
                var lobby = new Room(roomFiller.PlaceRoom(terrain, 8, 8, staticRooms.Select(r => r.Location), RoomPlacementConstraints.MapEdge), RoomType.Lobby);
                staticRooms.Add(lobby);
                var hallwayGen = new HallwayGenerator(rng);
                staticRooms.AddRange(hallwayGen.PlaceRandomHallway(terrain, lobby.Location, staticRooms.Select(r => r.Location).ToList(), 25)
                    .Select(l => new Room(l, RoomType.Hallway)));
            }
            else if (id == LevelId.AlwardsTower2)
            {
                // TODO up stairwell here
                // staticRooms.Add(new Room(roomFiller.PlaceRoom(terrain, 3, 3), RoomType.Stairwell));
                staticRooms.Add(new Room(roomFiller.PlaceRoom(terrain, 3, 3, staticRooms.Select(r => r.Location)), RoomType.Stairwell));
            }

            var staticLocations = staticRooms.Select(r => r.Location);
            var dynamicRoomLocations = roomFiller
                .FillRooms(terrain, 12, 2, 2, staticLocations);
            var roomLocations = dynamicRoomLocations
                .Concat(staticLocations);

            var doorGen = new DoorGenerator(rng);
            var doorsRound1 = doorGen.GenerateRandom(terrain, roomLocations);
            map.ApplyTerrainOverlay(terrain, MapFactory.SpawnDungeonTerrain);

            var doorsRound2 = doorGen.GenerateForWalkability(map, terrain, roomLocations);
            map.ApplyTerrainOverlay(terrain, MapFactory.SpawnDungeonTerrain);

            var name = id switch
            {
                LevelId.AlwardsTower1 => "Old Alward's Tower, floor 1",
                LevelId.AlwardsTower2 => "Old Alward's Tower, floor 2",
                _ => throw new ArgumentException($"Invalid level id {nameof(id)} for generator {nameof(AlwardsTowerLevelGenerator)}"),
            };

            var rooms = dynamicRoomLocations
                .Select(l => new Room(l, RoomType.None))
                .Concat(staticRooms)
                .ToList();

            var level = new Level(id, name, seed, rooms, doorsRound1.Concat(doorsRound2).ToList(), map);
            var meta = new LevelGenerationMetadata();

            return (level, meta);
        }

        private Level Generate(int seed, string id, IGenerator rng)
        {
            var (level, _) = GenerateTerrain(rng, seed, id, 30, 30);
            var map = level.Map;

            // spawn doors
            foreach (var door in level.Doors)
            {
                map.AddEntity(EntityFactory.CreateDoor(door));
            }

            // spawn floor-based doodads
            Coord spawnPosition;
            if (id == LevelId.AlwardsTower1)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var trapdoor = EntityFactory.CreateDoodad(spawnPosition, DoodadAtlas.Trapdoor);
                trapdoor.AddGoRogueComponent(new StoryMessageComponent(nameof(Story.AlwardsTower_TrapdoorStep), true));
                map.AddEntity(trapdoor);

                PopulateStairwellRooms(level, level.Rooms.Where(r => r.Type == RoomType.Stairwell).ToList(), null, LevelId.AlwardsTower2);
            }

            if (id == LevelId.AlwardsTower2)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var etheriumCore = EntityFactory.CreateDoodad(spawnPosition, DoodadAtlas.EtheriumCoreWithStand);
                map.AddEntity(etheriumCore);

                PopulateStairwellRooms(level, level.Rooms.Where(r => r.Type == RoomType.Stairwell).ToList(), LevelId.AlwardsTower1, null);
            }

            // populate rooms
            ClassifyRooms(level, rng);
            foreach (var room in level.Rooms)
            {
                switch (room.Type)
                {
                    case RoomType.Rubble:
                    case RoomType.Lobby:
                        PopulateRubbleRoom(level, room, rng);
                        break;
                    case RoomType.Study:
                        PopulateStudy(level, room, rng);
                        break;
                    case RoomType.Storeroom:
                        PopulateStoreroom(level, room, rng);
                        break;
                    case RoomType.Stairwell:
                        break;
                }
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

        private void ClassifyRooms(Level level, IGenerator rng)
        {
            var unclassifiedRooms = level.Rooms
                .Where(r => r.Type == RoomType.None)
                .ToList();

            foreach (var room in unclassifiedRooms)
            {
                if (room.Location.Width < 8
                    && room.Location.Height < 8
                    && rng.Next(100) < 15)
                {
                    room.Type = RoomType.Study;
                    continue;
                }

                if (rng.Next(100) < 15)
                {
                    room.Type = RoomType.Storeroom;
                    continue;
                }

                room.Type = RoomType.Rubble;
            }
        }

        private void PopulateStairwellRooms(
            Level level,
            IList<Room> rooms,
            string downLevelId,
            string upLevelId)
        {
            IList<Room> upRooms;
            if (downLevelId != null)
            {
                var spawnPosition = rooms[0].Location.Center;
                var stairDown = EntityFactory.CreateDoodad(spawnPosition, DoodadAtlas.StaircaseDown);
                stairDown.AddGoRogueComponent(new ChangeLevelComponent(downLevelId, new SpawnConditions(Spawn.StairUp, 0)));
                level.Map.AddEntity(stairDown);

                upRooms = rooms.Skip(1).ToList();
            }
            else
            {
                upRooms = rooms;
            }

            foreach (var room in upRooms)
            {
                var spawnPosition = room.Location.Center;
                var stairsUp = EntityFactory.CreateDoodad(spawnPosition, DoodadAtlas.StaircaseUp);
                stairsUp.AddGoRogueComponent(new ChangeLevelComponent(upLevelId, new SpawnConditions(Spawn.Stairdown, 0)));
                level.Map.AddEntity(stairsUp);
            }
        }

        private void PopulateRubbleRoom(Level level, Room room, IGenerator rng)
        {
            var doors = level.GetDoorsForRoom(room.Location);

            foreach (var pos in room.Location.Positions())
            {
                var canPlaceBlocker = PlacementRules.CanPlaceBlockingObject(pos, doors, level);
                PlaceRubble(pos, canPlaceBlocker, rng, level);
            }
        }

        private void PopulateStoreroom(Level level, Room room, IGenerator rng)
        {
            var doors = level.GetDoorsForRoom(room.Location);

            foreach (var pos in room.Location.Positions())
            {
                var canPlaceBlocker = PlacementRules.CanPlaceBlockingObject(pos, doors, level);
                var barrelRng = rng.Next(100);
                if (canPlaceBlocker
                    && (barrelRng < 5
                        || room.Location.IsOnPerimeter(pos) && barrelRng < 20))
                {
                    level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.SmallBarrel));
                    continue;
                }

                var chestRng = rng.Next(100);
                if (canPlaceBlocker
                    && (chestRng < 5
                        || room.Location.IsOnPerimeter(pos) && chestRng < 20))
                {
                    level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.SmallChest));
                    continue;
                }

                PlaceRubble(pos, canPlaceBlocker, rng, level);
            }
        }

        private void PopulateStudy(Level level, Room room, IGenerator rng)
        {
            var doors = level.GetDoorsForRoom(room.Location);

            foreach (var pos in room.Location.Positions())
            {
                var canPlaceBlocker = PlacementRules.CanPlaceBlockingObject(pos, doors, level);
                if (canPlaceBlocker
                    && room.Location.IsOnPerimeter(pos)
                    && rng.Next(100) < 25)
                {
                    level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.SmallBookshelf));
                    continue;
                }

                if (canPlaceBlocker
                    && rng.Next(100) < 5)
                {
                    level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.SmallDesk));
                    continue;
                }

                PlaceRubble(pos, canPlaceBlocker, rng, level);
            }
        }

        private void PlaceRubble(Coord pos, bool canPlaceBlocker, IGenerator rng, Level level)
        {
            if (canPlaceBlocker
                    && rng.Next(100) < 5)
            {
                level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.HeavyStoneRubble));
                return;
            }

            if (rng.Next(100) < 15)
            {
                level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.StoneRubble));
            }
        }
    }
}

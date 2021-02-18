using GoRogue;
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

            // spawn room-based doodads
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

            // populate rooms
            var rooms = ClassifyRooms(level, rng);
            foreach (var room in rooms)
            {
                switch (room.Type)
                {
                    case RoomType.Rubble:
                        PopulateRubbleRoom(level, room, rng);
                        break;
                    case RoomType.Study:
                        PopulateStudy(level, room, rng);
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

        private IEnumerable<Room> ClassifyRooms(Level level, IGenerator rng)
        {
            foreach (var room in level.Rooms)
            {
                if (room.Width < 8
                    && room.Height < 8
                    && rng.Next(100) < 15)
                {
                    yield return new Room(room, RoomType.Study);
                }

                yield return new Room(room, RoomType.Rubble);
            }
        }

        private void PopulateRubbleRoom(Level level, Room room, IGenerator rng)
        {
            var walls = room.Location.Expand(1, 1).PerimeterPositions();
            var doors = level.Doors.Where(d => walls.Contains(d));

            foreach (var pos in room.Location.Positions())
            {
                var canPlaceBlocker = PlacementRules.CanPlaceBlockingObject(pos, doors, level);
                if (canPlaceBlocker
                    && rng.Next(100) < 10)
                {
                    level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.HeavyStoneRubble));
                    continue;
                }

                if (rng.Next(100) < 25)
                {
                    level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.StoneRubble));
                }
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
                    && rng.Next(100) < 15)
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

                if (canPlaceBlocker
                    && rng.Next(100) < 5)
                {
                    level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.HeavyStoneRubble));
                    continue;
                }

                if (rng.Next(100) < 15)
                {
                    level.Map.AddEntity(EntityFactory.CreateDoodad(pos, DoodadAtlas.StoneRubble));
                }
            }
        }
    }
}

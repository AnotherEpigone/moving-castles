using GoRogue;
using GoRogue.MapViews;
using MovingCastles.Components.Levels;
using MovingCastles.Components.StoryComponents;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Saving;
using MovingCastles.Maps;
using MovingCastles.Maps.Generation;
using MovingCastles.Serialization.Map;
using MovingCastles.Text;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public class AlwardsTowerLevelGenerator : ILevelGenerator
    {
        private readonly IEntityFactory _entityFactory;
        private static readonly List<ItemTemplate> _floorItems = new List<ItemTemplate>()
        {
            ItemAtlas.EtheriumShard,
            ItemAtlas.SteelLongsword,
        };

        public AlwardsTowerLevelGenerator(IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
        }

        public string Id { get; } = "GENERATOR_ALWARDS_TOWER";

        public Level Generate(int seed, string id, PlayerInfo playerInfo, SpawnConditions playerSpawnConditions)
        {
            var rng = new StandardGenerator(seed);
            var level = Generate(seed, id, rng);

            // spawn player
            var spawnPosition = level.Map.WalkabilityView.RandomPosition(true, rng);
            var player = _entityFactory.CreatePlayer(spawnPosition, playerInfo);
            level.Map.AddEntity(player);

            return level;
        }

        public Level Generate(int seed, string id, PlayerInfo playerInfo, Coord playerSpawnPosition)
        {
            var rng = new StandardGenerator(seed);
            var level = Generate(seed, id, rng);

            // spawn player
            var player = _entityFactory.CreatePlayer(playerSpawnPosition, playerInfo);
            level.Map.AddEntity(player);

            return level;
        }

        public Level Generate(Save save)
        {
            var rng = new StandardGenerator(save.MapState.Seed);
            var level = Generate(save.MapState, rng);

            level.Map.AddEntity(save.Wizard);

            return level;
        }

        public Level Generate(MapState mapState, PlayerInfo playerInfo, SpawnConditions playerSpawnConditions)
        {
            var rng = new StandardGenerator(mapState.Seed);
            var level = Generate(mapState, rng);

            // spawn player
            var spawnPosition = level.Map.WalkabilityView.RandomPosition(true, rng);
            var player = _entityFactory.CreatePlayer(spawnPosition, playerInfo);
            level.Map.AddEntity(player);

            return level;
        }

        private Level Generate(int seed, string id, IGenerator rng)
        {
            var level = GenerateTerrainWithDoorLocations(rng, seed, id, 30, 30);
            var map = level.Map;

            // spawn doodads
            Coord spawnPosition;

            if (id == LevelId.AlwardsTower1)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var trapdoor = _entityFactory.CreateDoodad(spawnPosition, DoodadAtlas.Trapdoor);
                trapdoor.AddGoRogueComponent(new StoryMessageBoxComponent(nameof(Story.AlwardsTower_TrapdoorStep), true));
                map.AddEntity(trapdoor);

                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var stairsUp = _entityFactory.CreateDoodad(spawnPosition, DoodadAtlas.StaircaseUp);
                stairsUp.AddGoRogueComponent(new ChangeLevelComponent(LevelId.AlwardsTower2, new SpawnConditions(Spawn.Stairdown, 0)));
                map.AddEntity(stairsUp);
            }

            if (id == LevelId.AlwardsTower2)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var etheriumCore = _entityFactory.CreateDoodad(spawnPosition, DoodadAtlas.EtheriumCoreWithStand);
                map.AddEntity(etheriumCore);

                spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
                var stairDown = _entityFactory.CreateDoodad(spawnPosition, DoodadAtlas.StaircaseDown);
                stairDown.AddGoRogueComponent(new ChangeLevelComponent(LevelId.AlwardsTower1, new SpawnConditions(Spawn.StairUp, 0)));
                map.AddEntity(stairDown);
            }

            // spawn doors
            foreach (var door in level.Doors)
            {
                map.AddEntity(_entityFactory.CreateDoor(door));
            }

            // Spawn enemies
            var allTheActors = ActorAtlas.ActorsById.Values.ToList();
            for (int i = 0; i < 10; i++)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true);

                var enemy = _entityFactory.CreateActor(spawnPosition, allTheActors.RandomItem());
                map.AddEntity(enemy);
            }

            // Spawn items
            for (int i = 0; i < 10; i++)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true);

                var item = _entityFactory.CreateItem(spawnPosition, _floorItems.RandomItem());

                map.AddEntity(item);
            }

            return level;
        }

        private Level Generate(MapState mapState, IGenerator rng)
        {
            var level = GenerateTerrainWithDoorLocations(
                rng,
                mapState.Seed,
                mapState.Id,
                mapState.Width,
                mapState.Height);

            // restore terrain before entities
            level.Map.Explored = new ArrayMap<bool>(mapState.Explored, mapState.Width);
            level.Map.FovVisibilityHandler.RefreshExploredTerrain();

            foreach (var entity in mapState.Entities)
            {
                level.Map.AddEntity(entity);
            }

            foreach (var door in mapState.Doors)
            {
                level.Map.AddEntity(door);
            }

            return level;
        }

        private Level GenerateTerrainWithDoorLocations(IGenerator rng, int seed, string id, int width, int height)
        {
            var map = new DungeonMap(width, height);
            var terrain = new ArrayMap<bool>(width, height);

            var rooms = new RoomFiller(rng).Generate(terrain, 12, 2, 2);

            var doorGen = new DoorGenerator(rng);
            var doorsRound1 = doorGen.GenerateRandom(terrain, rooms);
            map.ApplyTerrainOverlay(terrain, MapFactory.SpawnDungeonTerrain);

            var doorsRound2 = doorGen.GenerateForWalkability(map, terrain, rooms);
            map.ApplyTerrainOverlay(terrain, MapFactory.SpawnDungeonTerrain);

            return new Level(id, seed, rooms, doorsRound1.Concat(doorsRound2).ToList(), map);
        }
    }
}

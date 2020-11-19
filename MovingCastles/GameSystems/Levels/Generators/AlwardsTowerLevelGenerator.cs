using GoRogue;
using GoRogue.MapViews;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Items;
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

        public Level Generate(int seed, PlayerInfo playerInfo)
        {
            var level = GenerateTerrainWithDoorLocations(seed, 30, 30);
            var map = level.Map;

            var rng = new StandardGenerator(seed);

            // spawn doodads
            var spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
            var trapdoor = _entityFactory.CreateDoodad(spawnPosition, DoodadAtlas.Trapdoor);
            map.AddEntity(trapdoor);

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

            // spawn player
            spawnPosition = map.WalkabilityView.RandomPosition(true, rng);
            var player = _entityFactory.CreatePlayer(spawnPosition, playerInfo);
            map.AddEntity(player);

            return level;
        }

        public Level Generate(Save save)
        {
            var level = GenerateTerrainWithDoorLocations(
                save.MapState.Seed,
                save.MapState.Width,
                save.MapState.Height);

            // restore terrain before entities
            level.Map.Explored = new ArrayMap<bool>(save.MapState.Explored, save.MapState.Width);
            level.Map.FovVisibilityHandler.RefreshExploredTerrain();

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

        private Level GenerateTerrainWithDoorLocations(int seed, int width, int height)
        {
            var rng = new StandardGenerator(seed);
            var map = new DungeonMap(width, height);
            var terrain = new ArrayMap<bool>(width, height);

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

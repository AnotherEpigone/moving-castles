﻿using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.Generators;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Player;
using MovingCastles.Maps.Generation;
using SadConsole;
using System.Linq;

namespace MovingCastles.Maps
{
    public class MapFactory : IMapFactory
    {
        private readonly IEntityFactory _entityFactory;

        public MapFactory(IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
        }

        public DungeonMap CreateDungeonMap(int width, int height, MapTemplate mapTemplate, Player playerInfo)
        {
            var map = new DungeonMap(width, height);

            // Generate map via GoRogue, and update the real map with appropriate terrain.
            var tempMap = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateRandomRoomsMap(tempMap, maxRooms: 180, roomMinSize: 8, roomMaxSize: 12);
            map.ApplyTerrainOverlay(tempMap, SpawnDungeonTerrain);

            Coord spawnPosition;

            // Spawn a few mock enemies
            var allTheActors = ActorAtlas.ActorsById.Values.ToList();
            for (int i = 0; i < 30; i++)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true); // Get a location that is walkable

                var goblin = _entityFactory.CreateActor(spawnPosition, allTheActors.RandomItem());
                map.AddEntity(goblin);
            }

            // Spawn a few items
            for (int i = 0; i < 30; i++)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true);

                var item = _entityFactory.CreateItem(spawnPosition, mapTemplate.FloorItems.ConvertAll(i => ItemAtlas.ItemsById[i]).RandomItem());

                map.AddEntity(item);
            }

            // Spawn player
            spawnPosition = map.WalkabilityView.RandomPosition(true);

            var player = _entityFactory.CreatePlayer(spawnPosition, playerInfo);
            map.AddEntity(player);

            return map;
        }

        public CastleMap CreateCastleMap(int width, int height, MapTemplate mapPlan, Player playerInfo)
        {
            var map = new CastleMap(width, height);

            // Generate map via GoRogue, and update the real map with appropriate terrain.
            var tempMap = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateRectangleMap(tempMap);
            map.ApplyTerrainOverlay(tempMap, SpawnOutdoorTerrain);

            var spawnPosition = new Point(3, 3);

            var castle = _entityFactory.CreateCastle(spawnPosition, playerInfo);
            map.AddEntity(castle);

            return map;
        }

        public DungeonMap CreateMapGenTestAreaMap(int width, int height, MapTemplate mapPlan, Player playerInfo)
        {
            var map = new DungeonMap(width, height);

            // blank canvas
            var emptyMapTerrain = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateRectangleMap(emptyMapTerrain);
            map.ApplyTerrainOverlay(emptyMapTerrain, SpawnOutdoorTerrain);

            // add a 20x20 maze with an entrance
            var mazeTerrain = new ArrayMap<bool>(20, 20);
            new BorderlessMazeGenerator().Generate(mazeTerrain);
            map.ApplyTerrainOverlay(mazeTerrain, new Coord(2, 2), SpawnDungeonTerrain);

            // add a 20x20 forest, hopefully with a path through it (a path! a path!)
            var forestPathTerrain = new ArrayMap<bool>(20, 20);
            new ForestPathGenerator().Generate(forestPathTerrain);
            map.ApplyTerrainOverlay(forestPathTerrain, new Coord(24, 2), SpawnOutdoorTerrain);

            // spawn player
            var spawnPosition = new Coord(23, 1);
            var player = _entityFactory.CreatePlayer(spawnPosition, playerInfo);
            map.AddEntity(player);

            // No FOV by default
            map.FovVisibilityHandler.Disable();

            return map;
        }

        private static IGameObject SpawnDungeonTerrain(Coord position, bool mapGenValue)
        {
            if (mapGenValue)
            {
                // Floor
                return new BasicTerrain(Color.White, new Color(41, 25, 40, 255), SpriteAtlas.Ground_Dirt, position, isWalkable: true, isTransparent: true);
            }
            else
            {
                // Wall
                return new BasicTerrain(Color.White, new Color(41, 25, 40, 255), SpriteAtlas.Wall_Brick, position, isWalkable: false, isTransparent: false);
            }
        }

        private static IGameObject SpawnOutdoorTerrain(Coord position, bool mapGenValue)
        {
            if (mapGenValue)
            {
                // Floor
                return new BasicTerrain(Color.White, new Color(41, 25, 40, 255), SpriteAtlas.Ground_Dirt2, position, isWalkable: true, isTransparent: true);
            }
            else
            {
                // Wall
                return new BasicTerrain(Color.White, new Color(41, 25, 40, 255), SpriteAtlas.Forest, position, isWalkable: true, isTransparent: false);
            }
        }
    }
}

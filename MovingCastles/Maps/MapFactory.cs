using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.Player;
using SadConsole;

namespace MovingCastles.Maps
{
    public class MapFactory : IMapFactory
    {
        private readonly IEntityFactory _entityFactory;

        public MapFactory(IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
        }

        public DungeonMap CreateDungeonMap(int width, int height, IMapPlan mapPlan, Player playerInfo)
        {
            var map = new DungeonMap(width, height);

            // Generate map via GoRogue, and update the real map with appropriate terrain.
            var tempMap = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateRandomRoomsMap(tempMap, maxRooms: 180, roomMinSize: 8, roomMaxSize: 12);
            map.ApplyTerrainOverlay(tempMap, SpawnDungeonTerrain);

            Coord spawnPosition;

            // Spawn a few mock enemies
            for (int i = 0; i < 30; i++)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true); // Get a location that is walkable

                var goblin = _entityFactory.CreateActor(SpriteAtlas.Goblin, spawnPosition, "Goblin");
                map.AddEntity(goblin);
            }

            // Spawn a few items
            for (int i = 0; i < 30; i++)
            {
                spawnPosition = map.WalkabilityView.RandomPosition(true);

                var item = _entityFactory.CreateItem(spawnPosition, mapPlan.FloorItems.RandomItem());

                map.AddEntity(item);
            }

            // Spawn player
            spawnPosition = map.WalkabilityView.RandomPosition(true);

            var player = _entityFactory.CreatePlayer(spawnPosition, playerInfo);
            map.AddEntity(player);

            return map;
        }

        public CastleMap CreateCastleMap(int width, int height, IMapPlan mapPlan, Player playerInfo)
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

        private static IGameObject SpawnDungeonTerrain(Coord position, bool mapGenValue)
        {
            // Floor or wall.  This could use the Factory system, or instantiate Floor and Wall classes, or something else if you prefer;
            // this simplistic if-else is just used for example
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
            // Floor or wall.  This could use the Factory system, or instantiate Floor and Wall classes, or something else if you prefer;
            // this simplistic if-else is just used for example
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

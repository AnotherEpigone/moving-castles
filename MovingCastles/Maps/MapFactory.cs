using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.Fonts;
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

        public MovingCastlesMap Create(int width, int height, IMapPlan mapPlan)
        {
            var map = new MovingCastlesMap(width, height);

            // Generate map via GoRogue, and update the real map with appropriate terrain.
            var tempMap = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateRandomRoomsMap(tempMap, maxRooms: 180, roomMinSize: 8, roomMaxSize: 12);
            map.ApplyTerrainOverlay(tempMap, SpawnTerrain);

            Coord posToSpawn;

            // Spawn a few mock enemies
            for (int i = 0; i < 30; i++)
            {
                posToSpawn = map.WalkabilityView.RandomPosition(true); // Get a location that is walkable

                var goblin = _entityFactory.CreateActor(SpriteAtlas.Goblin, posToSpawn, "Goblin");
                map.AddEntity(goblin);
            }

            // Spawn a few items
            for (int i = 0; i < 30; i++)
            {
                posToSpawn = map.WalkabilityView.RandomPosition(true);

                var item = _entityFactory.CreateItem(posToSpawn, mapPlan.FloorItems.RandomItem());

                map.AddEntity(item);
            }

            // Spawn player
            posToSpawn = map.WalkabilityView.RandomPosition(true);

            var player = _entityFactory.CreatePlayer(posToSpawn);
            map.AddEntity(player);

            return map;
        }

        private static IGameObject SpawnTerrain(Coord position, bool mapGenValue)
        {
            // Floor or wall.  This could use the Factory system, or instantiate Floor and Wall classes, or something else if you prefer;
            // this simplistic if-else is just used for example
            if (mapGenValue)
            {
                // Floor
                return new BasicTerrain(Color.White, new Color(61, 35, 50, 255), SpriteAtlas.Ground_Dirt, position, isWalkable: true, isTransparent: true);
            }
            else
            {
                // Wall
                return new BasicTerrain(Color.White, new Color(61, 35, 50, 255), SpriteAtlas.Wall_Brick, position, isWalkable: false, isTransparent: false);
            }
        }
    }
}

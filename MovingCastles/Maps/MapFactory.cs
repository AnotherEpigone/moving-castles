using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.Fonts;
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

        public CastleMap CreateCastleMap(int width, int height, MapTemplate mapPlan, Wizard player)
        {
            var map = new CastleMap(width, height);

            // Generate map via GoRogue, and update the real map with appropriate terrain.
            var tempMap = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateRectangleMap(tempMap);
            map.ApplyTerrainOverlay(tempMap, SpawnOutdoorTerrain);

            player.Position = new Point(3, 3);
            map.AddEntity(player);

            return map;
        }

        public static IGameObject SpawnDungeonTerrain(Coord position, bool mapGenValue)
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

        public static IGameObject SpawnOutdoorTerrain(Coord position, bool mapGenValue)
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

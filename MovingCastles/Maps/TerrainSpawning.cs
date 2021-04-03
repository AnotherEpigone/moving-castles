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
    public static class TerrainSpawning
    {
        public static IGameObject SpawnDungeonTerrain(Coord position, bool mapGenValue)
        {
            return mapGenValue
                ? new BasicTerrain(Color.White, new Color(41, 25, 40, 255), DungeonModeSpriteAtlas.Ground_Dirt, position, isWalkable: true, isTransparent: true)
                : new BasicTerrain(Color.White, new Color(41, 25, 40, 255), DungeonModeSpriteAtlas.Wall_Brick, position, isWalkable: false, isTransparent: false);
        }

        public static IGameObject SpawnOutdoorTerrain(Coord position, bool mapGenValue)
        {
            return mapGenValue
                ? new BasicTerrain(Color.White, new Color(41, 25, 40, 255), DungeonModeSpriteAtlas.Ground_Dirt2, position, isWalkable: true, isTransparent: true)
                : new BasicTerrain(Color.White, new Color(41, 25, 40, 255), DungeonModeSpriteAtlas.Forest, position, isWalkable: false, isTransparent: false);
        }

        public static IGameObject SpawnMountainTerrain(Coord position, bool mapGenValue)
        {
            return mapGenValue
                ? new BasicTerrain(Color.White, new Color(41, 25, 40, 255), CastleModeSpriteAtlas.Ground_Dirt, position, isWalkable: true, isTransparent: true)
                : new BasicTerrain(Color.White, new Color(41, 25, 40, 255), CastleModeSpriteAtlas.Wall_Rock, position, isWalkable: false, isTransparent: false);
        }
    }
}

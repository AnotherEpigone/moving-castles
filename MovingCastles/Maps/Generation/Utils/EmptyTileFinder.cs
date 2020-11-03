using GoRogue;
using GoRogue.MapViews;
using System.Runtime.CompilerServices;
using Troschuetz.Random;

namespace MovingCastles.Maps.Generation.Utils
{
    public static class EmptyTileFinder
    {
        public static Coord Find(ISettableMapView<bool> map, IGenerator rng)
        {
            // Try random positions first
            for (int i = 0; i < 100; i++)
            {
                var location = map.RandomPosition(false, rng);

                if (IsPointConsideredEmpty(map, location))
                {
                    return location;
                }
            }

            // Start looping through every single one
            for (int i = 0; i < map.Width * map.Height; i++)
            {
                var location = Coord.ToCoord(i, map.Width);
                if (IsPointConsideredEmpty(map, location))
                {
                    return location;
                }
            }

            return Coord.NONE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsPointConsideredEmpty(IMapView<bool> map, Coord location)
        {
            return IsPointSurroundedByWall(map, location) && // make sure is surrounded by a wall.
                   !map[location]; // The location is a wall
        }

        private static bool IsPointSurroundedByWall(IMapView<bool> map, Coord location)
        {
            var points = AdjacencyRule.EIGHT_WAY.Neighbors(location);

            var mapBounds = map.Bounds();
            foreach (var point in points)
            {
                if (!mapBounds.Contains(point))
                    continue;

                if (map[point])
                    return false;
            }

            return true;
        }
    }
}

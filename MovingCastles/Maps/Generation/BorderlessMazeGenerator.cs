using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using GoRogue.Random;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Troschuetz.Random;

namespace MovingCastles.Maps.Generation
{
    /// <summary>
    /// Generates a maze, and adds it to the given map.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Based on the MazeGenerator from GoRogue, which in turn is based on
    /// http://journal.stuffwithstuff.com/2014/12/21/rooms-and-mazes/
    /// </para>
    /// <para>However, this one doesn't leave a border of walls around the outside</para>
    /// </remarks>
    public static class BorderlessMazeGenerator
    {
        /// <summary>
        /// Generates a maze in map using crawlers that walk the map carving tunnels.
        /// </summary>
        /// <param name="map">The map to modify.</param>
        /// <param name="crawlerChangeDirectionImprovement">
        /// Out of 100, how much to increase the chance of the crawler changing direction each step.
        /// Once it changes direction, the chance resets to 0 and increases by this amount. Defaults
        /// to 10.
        /// </param>
        /// <returns>A list of mazes that were generated.</returns>
        public static IEnumerable<MapArea> Generate(ISettableMapView<bool> map, int crawlerChangeDirectionImprovement = 10)
            => Generate(map, null, crawlerChangeDirectionImprovement);

        /// <summary>
        /// Generates a maze in map using crawlers that walk the map carving tunnels.
        /// </summary>
        /// <param name="map">The map to modify.</param>
        /// <param name="rng">The RNG to use.</param>
        /// <param name="crawlerChangeDirectionImprovement">
        /// Out of 100, how much to increase the chance of the crawler changing direction each step.
        /// Once it changes direction, the chance resets to 0 and increases by this amount. Defaults
        /// to 10.
        /// </param>
        /// <returns>A list of mazes that were generated.</returns>
        public static IEnumerable<MapArea> Generate(ISettableMapView<bool> map, IGenerator rng, int crawlerChangeDirectionImprovement = 10)
        {
            if (rng == null)
            {
                rng = SingletonRandom.DefaultRNG;
            }

            var crawlers = new List<Crawler>();

            var empty = FindBorderSquare(map, rng); // start from border

            while (empty != Coord.NONE)
            {
                Crawler crawler = new Crawler();
                crawlers.Add(crawler);
                crawler.MoveTo(empty);
                var startedCrawler = true;
                var percentChangeDirection = 0;

                while (crawler.Path.Count != 0)
                {
                    // Dig this position
                    map[crawler.CurrentPosition] = true;

                    // Get valid directions (basically is any position outside the map or not?)
                    var points = AdjacencyRule.CARDINALS.NeighborsClockwise(crawler.CurrentPosition).ToArray();
                    var directions = AdjacencyRule.CARDINALS.DirectionsOfNeighborsClockwise(Direction.NONE).ToList();
                    var valids = new bool[4];

                    // Rule out any valids based on their position. Only process NSEW, do not use diagonals
                    for (var i = 0; i < 4; i++)
                    {
                        valids[i] = IsPointWallsExceptSource(map, points[i], directions[i] + 4);
                    }

                    // If not a new crawler, exclude where we came from
                    if (!startedCrawler)
                    {
                        valids[directions.IndexOf(crawler.Facing + 4)] = false;
                    }

                    // Do we have any valid direction to go?
                    if (valids[0] || valids[1] || valids[2] || valids[3])
                    {
                        var index = 0;

                        // Are we just starting this crawler? OR Is the current crawler facing
                        // direction invalid?
                        if (startedCrawler || valids[directions.IndexOf(crawler.Facing)] == false)
                        {
                            // Just get anything
                            index = GetDirectionIndex(valids, rng);
                            crawler.Facing = directions[index];
                            percentChangeDirection = 0;
                            startedCrawler = false;
                        }
                        else
                        {
                            // Increase probablity we change direction
                            percentChangeDirection += crawlerChangeDirectionImprovement;

                            if (PercentageCheck(percentChangeDirection, rng))
                            {
                                index = GetDirectionIndex(valids, rng);
                                crawler.Facing = directions[index];
                                percentChangeDirection = 0;
                            }
                            else
                            {
                                index = directions.IndexOf(crawler.Facing);
                            }
                        }

                        crawler.MoveTo(points[index]);
                    }
                    else
                    {
                        crawler.Backtrack();
                    }
                }

                empty = FindEmptySquare(map, rng);
            }

            return crawlers.Select(c => c.AllPositions).Where(a => a.Count != 0);
        }

        private static Coord FindBorderSquare(IMapView<bool> map, IGenerator rng)
        {
            var x = (int)rng.NextUInt((uint)map.Width - 1);
            var y = (int)rng.NextUInt((uint)map.Height - 1);
            switch (rng.NextBoolean())
            {
                case true:
                    x = x < map.Width / 2
                        ? 0
                        : map.Width - 1;
                    break;
                case false:
                    y = y < map.Width / 2
                        ? 0
                        : map.Height - 1;
                    break;
            }

            return new Coord(x, y);
        }

        private static Coord FindEmptySquare(IMapView<bool> map, IGenerator rng)
        {
            // Try random positions first
            for (int i = 0; i < 100; i++)
            {
                var location = map.RandomPosition(false, rng);

                if (IsPointConsideredEmpty(map, location))
                    return location;
            }

            // Start looping through every single one
            for (int i = 0; i < map.Width * map.Height; i++)
            {
                var location = Coord.ToCoord(i, map.Width);

                if (IsPointConsideredEmpty(map, location))
                    return location;
            }

            return Coord.NONE;
        }

        private static int GetDirectionIndex(bool[] valids, IGenerator rng)
        {
            // 10 tries to find random ok valid
            bool randomSuccess = false;
            int tempDirectionIndex = 0;

            for (int randomCounter = 0; randomCounter < 10; randomCounter++)
            {
                tempDirectionIndex = rng.Next(4);
                if (valids[tempDirectionIndex])
                {
                    randomSuccess = true;
                    break;
                }
            }

            // Couldn't find an active valid, so just run through each
            if (!randomSuccess)
            {
                if (valids[0])
                    tempDirectionIndex = 0;
                else if (valids[1])
                    tempDirectionIndex = 1;
                else if (valids[2])
                    tempDirectionIndex = 2;
                else
                    tempDirectionIndex = 3;
            }

            return tempDirectionIndex;
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

        private static bool IsPointWallsExceptSource(IMapView<bool> map, Coord location, Direction sourceDirection)
        {
            // Get map indexes for all surrounding locations
            var index = AdjacencyRule.EIGHT_WAY.DirectionsOfNeighborsClockwise().ToArray();

            Direction[] skipped;

            if (sourceDirection == Direction.RIGHT)
                skipped = new[] { sourceDirection, Direction.UP_RIGHT, Direction.DOWN_RIGHT };
            else if (sourceDirection == Direction.LEFT)
                skipped = new[] { sourceDirection, Direction.UP_LEFT, Direction.DOWN_LEFT };
            else if (sourceDirection == Direction.UP)
                skipped = new[] { sourceDirection, Direction.UP_RIGHT, Direction.UP_LEFT };
            else
                skipped = new[] { sourceDirection, Direction.DOWN_RIGHT, Direction.DOWN_LEFT };

            for (int i = 0; i < index.Length; i++)
            {
                if (skipped[0] == index[i] || skipped[1] == index[i] || skipped[2] == index[i])
                    continue;

                if (!map.Bounds().Contains(location + index[i]) || map[location + index[i]])
                    return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool PercentageCheck(int outOfHundred, IGenerator rng) => outOfHundred > 0 && rng.Next(101) < outOfHundred;

        private class Crawler
        {
            public MapArea AllPositions = new MapArea();
            public Coord CurrentPosition = new Coord(0, 0);
            public Direction Facing = Direction.UP;
            public bool IsActive = true;
            public Stack<Coord> Path = new Stack<Coord>();

            public void Backtrack()
            {
                if (Path.Count != 0)
                    CurrentPosition = Path.Pop();
            }

            public void MoveTo(Coord position)
            {
                Path.Push(position);
                AllPositions.Add(position);
                CurrentPosition = position;
            }
        }
    }
}
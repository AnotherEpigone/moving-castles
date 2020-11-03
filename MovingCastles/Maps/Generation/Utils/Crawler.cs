using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Troschuetz.Random;

namespace MovingCastles.Maps.Generation.Utils
{
    public class Crawler
    {
        private readonly IGenerator _rng;
        private readonly int _changeDirectionIncrease;
        public MapArea AllPositions = new MapArea();
        public Coord CurrentPosition = new Coord(0, 0);
        public Direction Facing = Direction.UP;
        public bool IsActive = true;
        public Stack<Coord> Path = new Stack<Coord>();

        public Crawler(IGenerator rng, int changeDirectionIncrease)
        {
            _rng = rng;
            _changeDirectionIncrease = changeDirectionIncrease;
        }

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

        public void Crawl(Coord start, ISettableMapView<bool> map)
        {
            MoveTo(start);

            var startedCrawler = true;
            var percentChangeDirection = 0;

            while (Path.Count != 0)
            {
                // Dig this position
                map[CurrentPosition] = true;

                // Get valid directions (basically is any position outside the map or not?)
                var points = AdjacencyRule.CARDINALS.NeighborsClockwise(CurrentPosition).ToArray();
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
                    valids[directions.IndexOf(Facing + 4)] = false;
                }

                // Do we have any valid direction to go?
                if (valids[0] || valids[1] || valids[2] || valids[3])
                {
                    int index;

                    // Are we just starting this crawler? OR Is the current crawler facing
                    // direction invalid?
                    if (startedCrawler || valids[directions.IndexOf(Facing)] == false)
                    {
                        // Just get anything
                        index = GetDirectionIndex(valids);
                        Facing = directions[index];
                        percentChangeDirection = 0;
                        startedCrawler = false;
                    }
                    else
                    {
                        // Increase probablity we change direction
                        percentChangeDirection += _changeDirectionIncrease;

                        if (PercentageCheck(percentChangeDirection, _rng))
                        {
                            index = GetDirectionIndex(valids);
                            Facing = directions[index];
                            percentChangeDirection = 0;
                        }
                        else
                        {
                            index = directions.IndexOf(Facing);
                        }
                    }

                    MoveTo(points[index]);
                }
                else
                {
                    Backtrack();
                }
            }
        }

        private static bool IsPointWallsExceptSource(IMapView<bool> map, Coord location, Direction sourceDirection)
        {
            // Get map indexes for all surrounding locations
            var index = AdjacencyRule.EIGHT_WAY.DirectionsOfNeighborsClockwise().ToArray();

            Direction[] skipped;

            if (sourceDirection == Direction.RIGHT)
            {
                skipped = new[] { sourceDirection, Direction.UP_RIGHT, Direction.DOWN_RIGHT };
            }
            else if (sourceDirection == Direction.LEFT)
            {
                skipped = new[] { sourceDirection, Direction.UP_LEFT, Direction.DOWN_LEFT };
            }
            else if (sourceDirection == Direction.UP)
            {
                skipped = new[] { sourceDirection, Direction.UP_RIGHT, Direction.UP_LEFT };
            }
            else
            {
                skipped = new[] { sourceDirection, Direction.DOWN_RIGHT, Direction.DOWN_LEFT };
            }

            for (int i = 0; i < index.Length; i++)
            {
                if (skipped[0] == index[i] || skipped[1] == index[i] || skipped[2] == index[i])
                {
                    continue;
                }

                if (!map.Bounds().Contains(location + index[i]) || map[location + index[i]])
                {
                    return false;
                }
            }

            return true;
        }

        private int GetDirectionIndex(bool[] valids)
        {
            // 10 tries to find random ok valid
            bool randomSuccess = false;
            int tempDirectionIndex = 0;

            for (int randomCounter = 0; randomCounter < 10; randomCounter++)
            {
                tempDirectionIndex = _rng.Next(4);
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
        private static bool PercentageCheck(int outOfHundred, IGenerator rng) => outOfHundred > 0 && rng.Next(101) < outOfHundred;
    }
}

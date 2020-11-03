using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using GoRogue.Random;
using MovingCastles.Maps.Generation.Utils;
using System.Collections.Generic;
using System.Linq;
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
    /// <para>This one has an outside entrance, and isn't static.</para>
    /// </remarks>
    public class BorderlessMazeGenerator
    {
        private readonly IGenerator _rng;
        private readonly int _crawlerChangeDirectionIncrease;

        public BorderlessMazeGenerator()
            : this(SingletonRandom.DefaultRNG, 10) { }

        public BorderlessMazeGenerator(IGenerator rng, int crawlerChangeDirectionIncrease)
        {
            _rng = rng;
            _crawlerChangeDirectionIncrease = crawlerChangeDirectionIncrease;
        }

        public IEnumerable<MapArea> Generate(ISettableMapView<bool> map)
        {
            var crawlers = new List<Crawler>();

            var nextStartPos = FindBorderSquare(map); // start from border
            while (nextStartPos != Coord.NONE)
            {
                var crawler = new Crawler(_rng, _crawlerChangeDirectionIncrease);
                crawlers.Add(crawler);
                crawler.Crawl(nextStartPos, map);

                nextStartPos = EmptyTileFinder.Find(map, _rng);
            }

            return crawlers.Select(c => c.AllPositions).Where(a => a.Count != 0);
        }

        private Coord FindBorderSquare(IMapView<bool> map)
        {
            var x = (int)_rng.NextUInt((uint)map.Width - 1);
            var y = (int)_rng.NextUInt((uint)map.Height - 1);
            switch (_rng.NextBoolean())
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
    }
}
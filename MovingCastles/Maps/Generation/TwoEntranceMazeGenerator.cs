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
    public class TwoEntranceMazeGenerator
    {
        private readonly int _crawlerChangeDirectionIncrease;

        public TwoEntranceMazeGenerator()
            : this(10) { }

        public TwoEntranceMazeGenerator(
            int crawlerChangeDirectionIncrease)
        {
            _crawlerChangeDirectionIncrease = crawlerChangeDirectionIncrease;
        }

        public IEnumerable<MapArea> Generate(IGenerator rng, ISettableMapView<bool> map)
        {
            var crawlers = new List<Crawler>();

            // top entrance
            var nextStartPos = new Coord(rng.Next(map.Width), 0);
            var crawler = new Crawler(rng, _crawlerChangeDirectionIncrease);
            crawlers.Add(crawler);
            crawler.Crawl(nextStartPos, map);

            // bottom entrance
            nextStartPos = new Coord(rng.Next(map.Width), map.Height - 1);
            crawler = new Crawler(rng, _crawlerChangeDirectionIncrease);
            crawlers.Add(crawler);
            crawler.Crawl(nextStartPos, map);

            nextStartPos = EmptyTileFinder.Find(map, rng);

            return crawlers.Select(c => c.AllPositions).Where(a => a.Count != 0);
        }
    }
}

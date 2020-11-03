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
        private readonly IGenerator _rng;

        public TwoEntranceMazeGenerator()
            : this(SingletonRandom.DefaultRNG, 10) { }

        public TwoEntranceMazeGenerator(
            IGenerator rng,
            int crawlerChangeDirectionIncrease)
        {
            _crawlerChangeDirectionIncrease = crawlerChangeDirectionIncrease;
            _rng = rng;
        }

        public IEnumerable<MapArea> Generate(ISettableMapView<bool> map)
        {
            var crawlers = new List<Crawler>();

            // top entrance
            var nextStartPos = new Coord(_rng.Next(map.Width), 0);
            var crawler = new Crawler(_rng, _crawlerChangeDirectionIncrease);
            crawlers.Add(crawler);
            crawler.Crawl(nextStartPos, map);

            // bottom entrance
            nextStartPos = new Coord(_rng.Next(map.Width), map.Height - 1);
            crawler = new Crawler(_rng, _crawlerChangeDirectionIncrease);
            crawlers.Add(crawler);
            crawler.Crawl(nextStartPos, map);

            nextStartPos = EmptyTileFinder.Find(map, _rng);

            return crawlers.Select(c => c.AllPositions).Where(a => a.Count != 0);
        }
    }
}

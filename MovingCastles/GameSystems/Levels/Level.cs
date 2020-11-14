using GoRogue;
using MovingCastles.Maps;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Levels
{
    public class Level
    {
        public Level(
            int seed,
            IList<Rectangle> rooms,
            IList<Coord> doors,
            DungeonMap map)
        {
            Seed = seed;
            Rooms = rooms;
            Doors = doors;
            Map = map;
        }

        public int Seed { get; }

        public IList<Rectangle> Rooms { get; }

        public IList<Coord> Doors { get; }

        public DungeonMap Map { get; }
    }
}

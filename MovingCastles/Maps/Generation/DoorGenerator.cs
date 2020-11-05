using GoRogue;
using GoRogue.MapViews;
using GoRogue.Random;
using System.Collections.Generic;
using Troschuetz.Random;

namespace MovingCastles.Maps.Generation
{
    /// <summary>
    /// Basic door generator.
    /// Carves a door into every shared wall between any two rooms.
    /// </summary>
    public class DoorGenerator
    {
        private readonly IGenerator _rng;

        public DoorGenerator()
            : this(SingletonRandom.DefaultRNG) { }

        public DoorGenerator(IGenerator rng)
        {
            _rng = rng;
        }

        public IList<Coord> Generate(ISettableMapView<bool> map, IEnumerable<Rectangle> rooms)
        {
            var doors = new List<Coord>();
            var roomsToCheck = new List<Rectangle>(rooms);
            foreach (var room in rooms)
            {
                roomsToCheck.Remove(room);
                foreach (var neighbor in roomsToCheck)
                {
                    var intersection = GetWallIntersection(room, neighbor);
                    if (!intersection.IsEmpty)
                    {
                        doors.Add(GetDoorInWall(intersection));
                    }
                }
            }

            // carve
            foreach (var door in doors)
            {
                map[door] = true;
            }

            return doors;
        }

        private Coord GetDoorInWall(Rectangle wall)
        {
            return wall.Width == 1
                ? new Coord(wall.X, wall.Y + _rng.Next(wall.Height))
                : new Coord(wall.X + _rng.Next(wall.Width), wall.Y);
        }

        private Rectangle GetWallIntersection(Rectangle room1, Rectangle room2)
        {
            var intersection = Rectangle.GetIntersection(TopWall(room1), BottomWall(room2));
            if (!intersection.IsEmpty)
            {
                return intersection;
            }

            intersection = Rectangle.GetIntersection(BottomWall(room1), TopWall(room2));
            if (!intersection.IsEmpty)
            {
                return intersection;
            }

            intersection = Rectangle.GetIntersection(RightWall(room1), LeftWall(room2));
            if (!intersection.IsEmpty)
            {
                return intersection;
            }

            intersection = Rectangle.GetIntersection(LeftWall(room1), RightWall(room2));
            if (!intersection.IsEmpty)
            {
                return intersection;
            }

            return Rectangle.EMPTY;
        }

        private Rectangle TopWall(Rectangle room) => new Rectangle(room.X, room.Y - 1, room.Width, 1);
        private Rectangle BottomWall(Rectangle room) => new Rectangle(room.X, room.Y + room.Height, room.Width, 1);
        private Rectangle LeftWall(Rectangle room) => new Rectangle(room.X - 1, room.Y, 1, room.Height);
        private Rectangle RightWall(Rectangle room) => new Rectangle(room.X + room.Width, room.Y, 1, room.Height);
    }
}

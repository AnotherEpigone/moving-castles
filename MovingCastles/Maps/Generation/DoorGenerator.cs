using GoRogue;
using GoRogue.MapViews;
using GoRogue.Random;
using System.Collections.Generic;
using System.Linq;
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
        private readonly float _roomSkipChance;
        private readonly float _doorSkipChance;

        public DoorGenerator()
            : this(0.5f, 0.5f, SingletonRandom.DefaultRNG) { }

        public DoorGenerator(float roomSkipChance, float doorSkipChance, IGenerator rng)
        {
            _rng = rng;
            _roomSkipChance = roomSkipChance;
            _doorSkipChance = doorSkipChance;
        }

        public IList<Coord> Generate(ISettableMapView<bool> map, IEnumerable<Rectangle> rooms)
        {
            var doors = new List<Coord>();
            var roomsToCheck = new List<Rectangle>(rooms);
            var hasDoors = rooms.ToDictionary(
                r => r.Position,
                _ => false); // key: roomPos
            foreach (var room in rooms)
            {
                roomsToCheck.Remove(room);
                if (hasDoors[room.Position] && _rng.NextDouble() < _roomSkipChance)
                {
                    continue;
                }

                AddDoors(room, roomsToCheck, hasDoors, doors);

                // fallback in case the only neighbor was skipped
                // scan all neighbors for a door
                if (!hasDoors[room.Position])
                {
                    AddDoors(room, rooms, hasDoors, doors);
                }
            }

            // carve
            foreach (var door in doors)
            {
                map[door] = true;
            }

            return doors;
        }

        private void AddDoors(Rectangle room, IEnumerable<Rectangle> rooms, IDictionary<Coord, bool> hasDoors, List<Coord> doors)
        {
            foreach (var neighbor in rooms)
            {
                if (hasDoors[room.Position] && _rng.NextDouble() < _doorSkipChance)
                {
                    continue;
                }

                var intersection = GetWallIntersection(room, neighbor);
                if (!intersection.IsEmpty)
                {
                    doors.Add(GetDoorInWall(intersection));
                    hasDoors[room.Position] = true;
                    hasDoors[neighbor.Position] = true;
                }
            }
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

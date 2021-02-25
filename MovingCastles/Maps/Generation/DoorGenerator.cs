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
        private readonly int _maxWalkabilityDistance;

        public DoorGenerator(IGenerator rng)
            : this(0.6f, 0.8f, 50, rng) { }

        public DoorGenerator(float roomSkipChance, float doorSkipChance, int maxWalkabilityDistance, IGenerator rng)
        {
            _rng = rng;
            _roomSkipChance = roomSkipChance;
            _doorSkipChance = doorSkipChance;
            _maxWalkabilityDistance = maxWalkabilityDistance;
        }

        public IList<Coord> GenerateForWalkability(
            McMap map,
            ISettableMapView<bool> mapView,
            IEnumerable<Rectangle> rooms) => GenerateForWalkability(map, mapView, new Coord(0, 0), rooms);

        public IList<Coord> GenerateForWalkability(
            McMap map,
            ISettableMapView<bool> mapView,
            Coord mapViewOffset,
            IEnumerable<Rectangle> rooms)
        {
            var doors = new List<Coord>();
            var roomsToCheck = new List<Rectangle>(rooms);

            foreach (var room in rooms)
            {
                roomsToCheck.Remove(room);

                doors.AddRange(AddDoorsForWalkability(room, roomsToCheck, map, mapViewOffset));
            }

            // carve
            foreach (var door in doors)
            {
                mapView[door] = true;
            }

            return doors;
        }

        public IList<Coord> GenerateRandom(ISettableMapView<bool> map, IEnumerable<Rectangle> rooms)
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

                AddRandomDoors(room, roomsToCheck, hasDoors, doors);

                // fallback in case the only neighbor was skipped
                // scan all neighbors for a door
                if (!hasDoors[room.Position])
                {
                    AddRandomDoors(room, rooms, hasDoors, doors);
                }
            }

            // carve
            foreach (var door in doors)
            {
                map[door] = true;
            }

            return doors;
        }

        private void AddRandomDoors(Rectangle room, IEnumerable<Rectangle> rooms, IDictionary<Coord, bool> hasDoors, List<Coord> doors)
        {
            foreach (var neighbor in rooms)
            {
                if (hasDoors[room.Position] && _rng.NextDouble() < _doorSkipChance)
                {
                    continue;
                }

                var (intersection, _) = GetWallIntersection(room, neighbor);
                if (!intersection.IsEmpty)
                {
                    doors.Add(GetRandomPointInWall(intersection));
                    hasDoors[room.Position] = true;
                    hasDoors[neighbor.Position] = true;
                }
            }
        }

        private Coord GetRandomPointInWall(Rectangle wall)
        {
            return wall.Width == 1
                ? new Coord(wall.X, wall.Y + _rng.Next(wall.Height))
                : new Coord(wall.X + _rng.Next(wall.Width), wall.Y);
        }

        private List<Coord> AddDoorsForWalkability(Rectangle room, IEnumerable<Rectangle> rooms, McMap map, Coord mapViewOffset)
        {
            List<Coord> doors = new List<Coord>();
            foreach (var neighbor in rooms)
            {
                var (intersection, vertical) = GetWallIntersection(room, neighbor);
                if (!intersection.IsEmpty && !intersection.Positions().Any(p => map.WalkabilityView[p + mapViewOffset]))
                {
                    var intersectionPoint = GetRandomPointInWall(intersection);
                    var start = vertical
                        ? new Coord(intersectionPoint.X + 1, intersectionPoint.Y) + mapViewOffset
                        : new Coord(intersectionPoint.X, intersectionPoint.Y + 1) + mapViewOffset;
                    var end = vertical
                        ? new Coord(intersectionPoint.X - 1, intersectionPoint.Y) + mapViewOffset
                        : new Coord(intersectionPoint.X, intersectionPoint.Y - 1) + mapViewOffset;

                    var distance = map.AStar.ShortestPath(start, end)?.Length ?? double.MaxValue;
                    if (distance > _maxWalkabilityDistance)
                    {
                        doors.Add(intersectionPoint);
                    }
                }
            }

            return doors;
        }

        private (Rectangle, bool) GetWallIntersection(Rectangle room1, Rectangle room2)
        {
            var intersection = Rectangle.GetIntersection(TopWall(room1), BottomWall(room2));
            if (!intersection.IsEmpty)
            {
                return (intersection, false);
            }

            intersection = Rectangle.GetIntersection(BottomWall(room1), TopWall(room2));
            if (!intersection.IsEmpty)
            {
                return (intersection, false);
            }

            intersection = Rectangle.GetIntersection(RightWall(room1), LeftWall(room2));
            if (!intersection.IsEmpty)
            {
                return (intersection, true);
            }

            intersection = Rectangle.GetIntersection(LeftWall(room1), RightWall(room2));
            if (!intersection.IsEmpty)
            {
                return (intersection, true);
            }

            return (Rectangle.EMPTY, false);
        }

        private Rectangle TopWall(Rectangle room) => new Rectangle(room.X, room.Y - 1, room.Width, 1);
        private Rectangle BottomWall(Rectangle room) => new Rectangle(room.X, room.Y + room.Height, room.Width, 1);
        private Rectangle LeftWall(Rectangle room) => new Rectangle(room.X - 1, room.Y, 1, room.Height);
        private Rectangle RightWall(Rectangle room) => new Rectangle(room.X + room.Width, room.Y, 1, room.Height);
    }
}

using GoRogue;
using GoRogue.MapViews;
using System;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random;

namespace MovingCastles.Maps.Generation
{
    public class HallwayGenerator
    {
        private readonly IGenerator _rng;

        public HallwayGenerator(IGenerator rng)
        {
            _rng = rng;
        }

        public IEnumerable<Rectangle> PlaceRandomHallway(
            ISettableMapView<bool> map,
            Rectangle startRoom,
            IList<Rectangle> usedAreas,
            int minDistance)
        {
            List<Rectangle> rooms = null;
            for (int i = 0; i < 200; ++i)
            {
                var target = map.RandomPosition(_rng);
                if (Math.Abs(target.X - startRoom.Center.X) + Math.Abs(target.Y - startRoom.Center.Y) >= minDistance)
                {
                    var potentialRooms = PlaceHallway(startRoom.Expand(1, 1), target).ToList();
                    if (!potentialRooms.Any(r => CheckLocationConflicts(r, map, usedAreas)))
                    {
                        rooms = potentialRooms;
                        break;
                    }
                }
            }

            if (rooms == null)
            {
                foreach (var target in map.Positions())
                {
                    if (Math.Abs(target.X - startRoom.Center.X) + Math.Abs(target.Y - startRoom.Center.Y) >= minDistance)
                    {
                        var potentialRooms = PlaceHallway(startRoom.Expand(1, 1), target).ToList();
                        if (!potentialRooms.Any(r => CheckLocationConflicts(r, map, usedAreas)))
                        {
                            rooms = potentialRooms;
                            break;
                        }
                    }
                }

                if (rooms == null)
                {
                    throw new ArgumentException("Attempt to place hallway with no possible position.");
                }
            }

            // carve out the doorway between start room and hallway
            var expandedStartRoom = startRoom.Expand(1, 1);
            foreach (var room in rooms)
            {
                CarveRoom(room, map);
                var expandedSection = room.Width > room.Height
                    ? room.Expand(1, 0)
                    : room.Expand(0, 1);
                foreach (var overlapPos in expandedSection.Positions().Intersect(expandedStartRoom.Positions()))
                {
                    map[overlapPos] = true;
                }
            }

            return rooms;
        }

        public IEnumerable<Rectangle> PlaceHallway(
            Rectangle startRoom,
            Coord target)
        {
            if (target.Y > startRoom.Y && target.Y < startRoom.MaxExtentY)
            {
                // single horizontal section
                var startX = target.X > startRoom.Center.X
                    ? startRoom.MaxExtentX + 1
                    : startRoom.X - 1;
                var start = new Coord(startX, target.Y);
                yield return GetHallwaySection(start, target);
            }
            else if (target.X > startRoom.X && target.X < startRoom.MaxExtentX)
            {
                // single vertical section
                var startY = target.Y > startRoom.Center.Y
                    ? startRoom.MaxExtentY + 1
                    : startRoom.Y - 1;
                var start = new Coord(target.X, startY);
                yield return GetHallwaySection(start, target);
            }
            else if (_rng.Next(2) < 1)
            {
                // horizontal then vertical
                var edgePositions = target.X > startRoom.Center.X
                    ? startRoom.MaxXPositions().Select(p => new Coord(p.X + 1, p.Y))
                    : startRoom.MinXPositions().Select(p => new Coord(p.X - 1, p.Y));
                var start = edgePositions
                    .Skip(1)
                    .SkipLast(1)
                    .ToList()
                    .RandomItem(_rng);
                var intermediateTarget = new Coord(target.X, start.Y);
                yield return GetHallwaySection(start, intermediateTarget);
                var secondStart = intermediateTarget.Y > target.Y
                    ? new Coord(intermediateTarget.X, intermediateTarget.Y - 1)
                    : new Coord(intermediateTarget.X, intermediateTarget.Y + 1);
                yield return GetHallwaySection(secondStart, target);
            }
            else
            {
                // vertical then horizontal
                var edgePositions = target.Y > startRoom.Center.Y
                    ? startRoom.MaxYPositions().Select(p => new Coord(p.X, p.Y + 1))
                    : startRoom.MinYPositions().Select(p => new Coord(p.X, p.Y - 1));
                var start = edgePositions
                    .Skip(1)
                    .SkipLast(1)
                    .ToList()
                    .RandomItem(_rng);
                var intermediateTarget = new Coord(start.X, target.Y);
                yield return GetHallwaySection(start, intermediateTarget);
                var secondStart = intermediateTarget.X > target.X
                    ? new Coord(intermediateTarget.X - 1, intermediateTarget.Y)
                    : new Coord(intermediateTarget.X + 1, intermediateTarget.Y);
                yield return GetHallwaySection(secondStart, target);
            }
        }

        private static Rectangle GetHallwaySection(Coord a, Coord b)
        {
            if (a.X == b.X)
            {
                // vertical
                return new Rectangle(a.X, Math.Min(a.Y, b.Y), 1, Math.Abs(b.Y - a.Y) + 1);
            }

            if (a.Y == b.Y)
            {
                // horizontal
                return new Rectangle(Math.Min(a.X, b.X), a.Y, Math.Abs(b.X - a.X) + 1, 1);
            }

            throw new ArgumentException($"Hallway anchors don't form a straight line: ({a.X},{a.Y}) ({b.X},{b.Y}).");
        }

        private static bool CheckLocationConflicts(Rectangle room, ISettableMapView<bool> map, IList<Rectangle> rooms)
        {
            var expanded = room.Expand(1, 1);
            var derp = expanded.Positions().Any(pos => !map.Contains(pos) || map[pos]);
            var derp2 = rooms.Any(r => r.Intersects(expanded));
            var derp3 = expanded.Positions().Any(pos => !map.Contains(pos));
            return expanded.Positions().Any(pos => !map.Contains(pos) || map[pos])
                || rooms.Any(r => r.Intersects(expanded));
        }

        private static void CarveRoom(Rectangle room, ISettableMapView<bool> map)
        {
            foreach (var pos in room.Positions())
            {
                map[pos] = true;
            }
        }
    }
}

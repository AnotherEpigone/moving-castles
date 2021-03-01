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
            int minDistance,
            int width)
        {
            List<Rectangle> rooms = null;
            for (int i = 0; i < 200; ++i)
            {
                var target = map.RandomPosition(_rng);
                if (Math.Abs(target.X - startRoom.Center.X) + Math.Abs(target.Y - startRoom.Center.Y) >= minDistance)
                {
                    var potentialRooms = PlaceHallway(startRoom.Expand(1, 1), target, width).ToList();
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
                        var potentialRooms = PlaceHallway(startRoom.Expand(1, 1), target, width).ToList();
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
            Coord target,
            int width)
        {
            if (target.Y > startRoom.Y && target.Y < startRoom.MaxExtentY)
            {
                // single horizontal section
                var startX = target.X > startRoom.Center.X
                    ? startRoom.MaxExtentX + 1
                    : startRoom.X - 1;
                var start = new Coord(startX, target.Y);
                var effectiveTargetY = target.Y < startRoom.Center.Y
                    ? target.Y + width - 1
                    : target.Y - width + 1;
                var effectiveTarget = new Coord(target.X, effectiveTargetY);
                yield return GetHallwaySection(start, effectiveTarget);
            }
            else if (target.X > startRoom.X && target.X < startRoom.MaxExtentX)
            {
                // single vertical section
                var startY = target.Y > startRoom.Center.Y
                    ? startRoom.MaxExtentY + 1
                    : startRoom.Y - 1;
                var start = new Coord(target.X, startY);
                var effectiveTargetX = target.X < startRoom.Center.X
                    ? target.X + width - 1
                    : target.X - width + 1;
                var effectiveTarget = new Coord(effectiveTargetX, target.Y);
                yield return GetHallwaySection(start, effectiveTarget);
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
                var effectiveIntermediateTargetY = start.Y < startRoom.Center.Y
                    ? start.Y + width - 1
                    : start.Y - width + 1;
                var effectiveIntermediateTarget = new Coord(target.X, effectiveIntermediateTargetY);
                yield return GetHallwaySection(start, effectiveIntermediateTarget);
                var secondStart = intermediateTarget.Y > target.Y
                    ? new Coord(intermediateTarget.X, intermediateTarget.Y - 1)
                    : new Coord(intermediateTarget.X, intermediateTarget.Y + 1);
                var effectiveTargetX = target.X < startRoom.Center.X
                    ? target.X + width - 1
                    : target.X - width + 1;
                var effectiveTarget = new Coord(effectiveTargetX, target.Y);
                yield return GetHallwaySection(secondStart, effectiveTarget);
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
                var effectiveIntermediateTargetX = start.X < startRoom.Center.X
                    ? start.X + width - 1
                    : start.X - width + 1;
                var effectiveIntermediateTarget = new Coord(effectiveIntermediateTargetX, target.Y);
                yield return GetHallwaySection(start, effectiveIntermediateTarget);
                var secondStart = intermediateTarget.X > target.X
                    ? new Coord(intermediateTarget.X - 1, intermediateTarget.Y)
                    : new Coord(intermediateTarget.X + 1, intermediateTarget.Y);
                var effectiveTargetY = target.Y < startRoom.Center.Y
                    ? target.Y + width - 1
                    : target.Y - width + 1;
                var effectiveTarget = new Coord(target.X, effectiveTargetY);
                yield return GetHallwaySection(secondStart, effectiveTarget);
            }
        }

        private static Rectangle GetHallwaySection(Coord a, Coord b)
        {
            var minExtent = new Coord(Math.Min(a.X, b.X),
                                        Math.Min(a.Y, b.Y));
            var maxExtent = new Coord(Math.Max(a.X, b.X),
                                        Math.Max(a.Y, b.Y));
            return new Rectangle(minExtent, maxExtent);
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

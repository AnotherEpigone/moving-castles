using GoRogue;
using GoRogue.MapViews;
using GoRogue.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random;

namespace MovingCastles.Maps.Generation
{
    public class RoomFiller
    {
        private readonly IGenerator _rng;

        public RoomFiller()
            : this(SingletonRandom.DefaultRNG) { }

        public RoomFiller(IGenerator rng)
        {
            _rng = rng;
        }

        public Rectangle PlaceRoom(ISettableMapView<bool> map, int width, int height)
        {
            var roomRect = new Rectangle(0, 0, width, height);
            var rect = TryPlaceRoom(roomRect, map, new List<Rectangle>());
            if (rect == Rectangle.EMPTY)
            {
                rect = ForcePlaceRoom(roomRect, map, new List<Rectangle>());
            }

            CarveRoom(rect, map);
            return rect;
        }

        public IEnumerable<Rectangle> Generate(
            ISettableMapView<bool> map,
            int numberOfRooms,
            int minRoomWidth,
            int minRoomHeight,
            IEnumerable<Rectangle> usedAreas)
        {
            var minRoomRect = new Rectangle(0, 0, minRoomWidth, minRoomHeight);
            var rooms = new List<Rectangle>();
            for (int i = 0; i< numberOfRooms; i++)
            {
                var newRoom = TryPlaceRoom(minRoomRect, map, rooms.Concat(usedAreas));
                if (newRoom != Rectangle.EMPTY)
                {
                    rooms.Add(newRoom);
                }
            }

            ExpandRooms(map, rooms, usedAreas);
            foreach (var room in rooms)
            {
                CarveRoom(room, map);
            }

            return rooms;
        }

        private void ExpandRooms(ISettableMapView<bool> map, List<Rectangle> rooms, IEnumerable<Rectangle> usedAreas)
        {
            const int expansionPasses = 200;
            for (int i = 0; i < expansionPasses; i++)
            {
                var newRooms = new List<Rectangle>();
                foreach (var room in rooms)
                {
                    Rectangle expandedRoom = Rectangle.EMPTY;
                    switch (_rng.Next(4))
                    {
                        case 0:
                            expandedRoom = new Rectangle(room.X, room.Y, room.Width + 1, room.Height);
                            break;
                        case 1:
                            expandedRoom = new Rectangle(room.X - 1, room.Y, room.Width + 1, room.Height);
                            break;
                        case 2:
                            expandedRoom = new Rectangle(room.X, room.Y, room.Width, room.Height + 1);
                            break;
                        case 3:
                            expandedRoom = new Rectangle(room.X, room.Y - 1, room.Width, room.Height + 1);
                            break;
                    }

                    if (!CheckAdjacency(
                        expandedRoom,
                        map,
                        rooms
                            .Except(new Rectangle[] { room })
                            .Concat(newRooms)
                            .Concat(usedAreas)))
                    {
                        newRooms.Add(expandedRoom);
                    }
                    else
                    {
                        newRooms.Add(room);
                    }
                }

                rooms.Clear();
                rooms.AddRange(newRooms);
            }
        }

        private Rectangle TryPlaceRoom(Rectangle room, ISettableMapView<bool> map, IEnumerable<Rectangle> rooms)
        {
            const int placementAttempts = 200;
            for (int i = 0; i < placementAttempts; i++)
            {
                var pos = map.RandomPosition(_rng);
                var positionedRoom = room.WithPosition(pos);
                if (!CheckAdjacency(positionedRoom, map, rooms))
                {
                    return positionedRoom;
                }
            }

            return Rectangle.EMPTY;
        }

        private Rectangle ForcePlaceRoom(Rectangle room, ISettableMapView<bool> map, List<Rectangle> rooms)
        {
            foreach (var pos in map.Positions())
            {
                var positionedRoom = room.WithPosition(pos);
                if (!CheckAdjacency(positionedRoom, map, rooms))
                {
                    return positionedRoom;
                }
            }

            throw new ArgumentException("Attempt to force room placement with no possible position.");
        }

        private bool CheckAdjacency(Rectangle room, ISettableMapView<bool> map, IEnumerable<Rectangle> rooms)
        {
            var expanded = room.Expand(1, 1);
            return expanded.Positions().Any(pos => !map.Contains(pos) || map[pos])
                || rooms.Any(r => r.Intersects(expanded));
        }

        private void CarveRoom(Rectangle room, ISettableMapView<bool> map)
        {
            foreach (var pos in room.Positions())
            {
                map[pos] = true;
            }
        }
    }
}

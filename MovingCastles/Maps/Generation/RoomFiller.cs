using GoRogue;
using GoRogue.MapViews;
using GoRogue.Random;
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

        public IList<Rectangle> Generate(
            ISettableMapView<bool> map,
            int numberOfRooms,
            int minRoomWidth,
            int minRoomHeight)
        {
            var minRoomRect = new Rectangle(0, 0, minRoomWidth, minRoomHeight);
            var rooms = new List<Rectangle>();
            for (int i = 0; i< numberOfRooms; i++)
            {
                var newRoom = TryPlaceRoom(minRoomRect, map, rooms);
                if (newRoom != Rectangle.EMPTY)
                {
                    rooms.Add(newRoom);
                }
            }

            ExpandRooms(map, rooms);
            foreach (var room in rooms)
            {
                CarveRoom(room, map);
            }

            return rooms;
        }

        private void ExpandRooms(ISettableMapView<bool> map, List<Rectangle> rooms)
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
                            .Concat(newRooms)))
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

        private Rectangle TryPlaceRoom(Rectangle room, ISettableMapView<bool> map, List<Rectangle> rooms)
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

using GoRogue;
using MovingCastles.Maps;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.GameSystems.Levels
{
    public record Level(
            string Id,
            string Name,
            int Seed,
            IList<Room> Rooms,
            IList<Coord> Doors,
            DungeonMap Map)
    {
        public IEnumerable<Coord> GetDoorsForRoom(Rectangle room)
        {
            var walls = room.Expand(1, 1).PerimeterPositions();
            return Doors.Where(d => walls.Contains(d));
        }
    }
}

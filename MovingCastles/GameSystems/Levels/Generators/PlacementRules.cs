using GoRogue;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public static class PlacementRules
    {
        public static bool CanPlaceBlockingObject(Coord pos, IEnumerable<Coord> doors, Level level, Room room)
        {
            var doorList = doors.ToList();
            if (doorList.Count == 0)
            {
                return false;
            }

            // if adjacent to a door, or to more than 1 other blocked tile, this has the potential to
            // cut off pathing
            var dangerous = !AdjacencyRule.CARDINALS.Neighbors(pos).Any(n => doors.Contains(n))
                    && AdjacencyRule.CARDINALS.Neighbors(pos).Count(n => !level.Map.WalkabilityView[n]) < 2;
            if (!dangerous)
            {
                return true;
            }

            var firstDoor = doorList[0];
            if (doorList.Count == 1)
            {
                return AdjacencyRule.EIGHT_WAY.Neighbors(firstDoor).Any(p => room.Location.Contains(p) && level.Map.WalkabilityView[p]);
            }

            foreach (var door in doors.Skip(1))
            {
                if (level.Map.AStar.ShortestPath(firstDoor, door) == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

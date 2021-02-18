using GoRogue;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public static class PlacementRules
    {
        public static bool CanPlaceBlockingObject(Coord pos, IEnumerable<Coord> doors, Level level)
        {
            // can't be next to a door, or next to more than 1 other blocked tile
            return !AdjacencyRule.CARDINALS.Neighbors(pos).Any(n => doors.Contains(n))
                    && AdjacencyRule.CARDINALS.Neighbors(pos).Count(n => !level.Map.WalkabilityView[n]) < 2;
        }
    }
}

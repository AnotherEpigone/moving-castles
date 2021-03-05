using GoRogue;
using GoRogue.MapViews;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Extensions
{
    public static class IMapViewExtensions
    {
        public static bool CoordsReachable(
            this IMapView<bool> walkabilityView,
            IEnumerable<Coord> targetCoords,
            Rectangle searchBounds)
        {
            var startCoord = targetCoords.FirstOrDefault();
            if (startCoord == default(Coord))
            {
                return true;
            }

            var remainingCoords = targetCoords.Skip(1).ToList();
            if (remainingCoords.Count == 0)
            {
                return true;
            }

            var visitedCoords = new List<Coord> { startCoord };
            SearchNeighbors(startCoord, walkabilityView, remainingCoords, visitedCoords, searchBounds);
            return remainingCoords.Count == 0;
        }

        private static void SearchNeighbors(
            Coord basePos,
            IMapView<bool> walkabilityView,
            List<Coord> targetCoords,
            List<Coord> visitedCoords,
            Rectangle searchBounds)
        {
            foreach (var pos in AdjacencyRule.EIGHT_WAY.Neighbors(basePos))
            {
                if (visitedCoords.Contains(pos))
                {
                    continue;
                }

                visitedCoords.Add(pos);
                targetCoords.Remove(pos);

                if (targetCoords.Count == 0)
                {
                    return;
                }

                if (searchBounds.Contains(pos)
                    && walkabilityView[pos])
                {
                    SearchNeighbors(pos, walkabilityView, targetCoords, visitedCoords, searchBounds);
                }
            }
        }
    }
}

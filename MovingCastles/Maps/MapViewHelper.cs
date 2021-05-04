using GoRogue;
using GoRogue.MapViews;
using MovingCastles.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Maps
{
    public static class MapViewHelper
    {
        public static IMapView<bool> WalkableEmptyLayerView(McMap map, DungeonMapLayer layer)
        {
            return new LambdaMapView<bool>(
                map.Width,
                map.Height,
                c => map.WalkabilityView[c]
                    && map.GetEntity<McEntity>(c, LayerMasker.DEFAULT.Mask((int)layer)) == null);
        }

        public static IMapView<bool> MultiTileWalkableEmptyLayerView(McMap map, DungeonMapLayer layer, IEnumerable<Coord> subTileOffsets)
        {
            return new LambdaMapView<bool>(
                map.Width,
                map.Height,
                c => map.WalkabilityView[c]
                    && map.GetEntity<McEntity>(c, LayerMasker.DEFAULT.Mask((int)layer)) == null
                    && subTileOffsets.All(
                        st => map.WalkabilityView[c + st]
                        && map.GetEntity<McEntity>(c + st, LayerMasker.DEFAULT.Mask((int)layer)) == null));
        }
    }
}

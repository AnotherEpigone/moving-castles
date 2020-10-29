using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using SadConsole;
using System;

namespace MovingCastles.Maps
{
    public class McMap : BasicMap
    {
        public McMap(
            int width,
            int height,
            int numberOfEntityLayers,
            Distance distanceMeasurement,
            uint layersBlockingWalkability = uint.MaxValue,
            uint layersBlockingTransparency = uint.MaxValue,
            uint entityLayersSupportingMultipleItems = uint.MaxValue)
            : base(
                width,
                height,
                numberOfEntityLayers,
                distanceMeasurement,
                layersBlockingWalkability,
                layersBlockingTransparency,
                entityLayersSupportingMultipleItems)
        {
        }

        public void ApplyTerrainOverlay<T>(IMapView<T> overlay, Coord position, Func<Coord, T, IGameObject> translator)
        {
            foreach (var overlayPos in overlay.Positions())
            {
                var terrainVal = overlay[overlayPos];
                var adjustedPos = overlayPos + position;
                SetTerrain(translator(adjustedPos, terrainVal));
            }
        }
    }
}

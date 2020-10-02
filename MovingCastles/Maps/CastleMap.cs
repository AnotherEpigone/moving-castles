using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using SadConsole;
using System;
using System.Linq;

namespace MovingCastles.Maps
{
    internal enum CastleMapLayer
    {
        TERRAIN,
        ITEMS,
        MONSTERS,
        CASTLE
    }

    public class CastleMap : BasicMap
    {
        private readonly Lazy<Castle> _castle;

        public CastleMap(int width, int height)
            : base(
                  width,
                  height,
                  Enum.GetNames(typeof(DungeonMapLayer)).Length - 1,
                  Distance.CHEBYSHEV,
                  entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask((int)DungeonMapLayer.ITEMS))
        {
            // Note that passing *this* into the FOV handler sets up all kinds of FOV stuff in gorogue.
            // don't remove even if the property isn't used.
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);
            _castle = new Lazy<Castle>(() => Entities.Items.OfType<Castle>().First());
        }

        public FOVVisibilityHandler FovVisibilityHandler { get; }

        public Castle Castle => _castle.Value;
    }
}

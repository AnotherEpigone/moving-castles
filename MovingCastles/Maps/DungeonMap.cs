using System;
using System.Linq;
using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using SadConsole;

namespace MovingCastles.Maps
{
    internal enum DungeonMapLayer
    {
        TERRAIN,
        ITEMS,
        MONSTERS,
        PLAYER
    }

    public class DungeonMap : BasicMap
    {
        private readonly Lazy<Wizard> _player;

        public DungeonMap(int width, int height)
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
            _player = new Lazy<Wizard>(() => Entities.Items.OfType<Wizard>().First());
        }

        public FOVVisibilityHandler FovVisibilityHandler { get; }

        public Wizard Player => _player.Value;
    }
}

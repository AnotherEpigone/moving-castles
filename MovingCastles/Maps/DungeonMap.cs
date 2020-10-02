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
        private readonly Lazy<Player> _player;

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
            _player = new Lazy<Player>(() => Entities.Items.OfType<Player>().First());
        }

        public FOVVisibilityHandler FovVisibilityHandler { get; }

        public Player Player => _player.Value;
    }
}

using System;
using System.Linq;
using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using SadConsole;

namespace MovingCastles.Maps
{
    internal enum MapLayer
    {
        TERRAIN,
        ITEMS,
        MONSTERS,
        PLAYER
    }

    public class MovingCastlesMap : BasicMap
    {
        private readonly Lazy<Player> _player;

        public MovingCastlesMap(int width, int height)
            : base(
                  width,
                  height,
                  Enum.GetNames(typeof(MapLayer)).Length - 1,
                  Distance.CHEBYSHEV,
                  entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS))
        {
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);
            _player = new Lazy<Player>(() => Entities.Items.OfType<Player>().First());
        }

        public FOVVisibilityHandler FovVisibilityHandler { get; }

        public Player Player => _player.Value;
    }
}

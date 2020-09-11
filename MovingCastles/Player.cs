using System.Collections.Generic;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.Components;
using MovingCastles.Fonts;
using MovingCastles.Maps;
using SadConsole;

namespace MovingCastles
{
    // Custom class for the player is used in this example just so we can handle input.  This could be done via a component, or in a main screen, but for simplicity we do it here.
    internal class Player : BasicEntity
    {
        public int FOVRadius;

        public Player(Coord position, Font font)
            : base(Color.White,
                  Color.Transparent,
                  SpriteAtlas.PlayerDefault,
                  position,
                  (int)MapLayer.PLAYER,
                  isWalkable: false,
                  isTransparent: true)
        {
            FOVRadius = 10;
            Name = "Player";

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();

            AddGoRogueComponent(new InventoryComponent());
        }

        public void Move(Direction direction)
        {
            Position += direction;
        }
    }
}

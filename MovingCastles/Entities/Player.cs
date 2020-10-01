using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Fonts;
using MovingCastles.Maps;
using SadConsole;

namespace MovingCastles.Entities
{
    public sealed class Player : McEntity
    {
        public int FOVRadius;

        public Player(Coord position, Font font)
            : base("Player",
                  Color.White,
                  Color.Transparent,
                  SpriteAtlas.PlayerDefault,
                  position,
                  (int)MapLayer.PLAYER,
                  isWalkable: false,
                  isTransparent: true)
        {
            FOVRadius = 10;

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();

            AddGoRogueComponent(new MeleeAttackerComponent(5));
            AddGoRogueComponent(new HealthComponent(100));
            AddGoRogueComponent(new InventoryComponent());
        }
    }
}

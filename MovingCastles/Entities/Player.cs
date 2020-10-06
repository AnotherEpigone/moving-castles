using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.PlayerInfo;
using MovingCastles.Maps;
using SadConsole;

namespace MovingCastles.Entities
{
    public sealed class Player : McEntity
    {
        public int FOVRadius;

        public Player(Coord position, PlayerInfo playerInfo, Font font)
            : base(playerInfo.Name,
                  Color.White,
                  Color.Transparent,
                  SpriteAtlas.PlayerDefault,
                  position,
                  (int)DungeonMapLayer.PLAYER,
                  isWalkable: false,
                  isTransparent: true)
        {
            FOVRadius = 10;

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();

            AddGoRogueComponent(new MeleeAttackerComponent(5));
            AddGoRogueComponent(new HealthComponent(playerInfo.MaxHealth, playerInfo.Health));
            AddGoRogueComponent(new InventoryComponent(playerInfo.Items.ToArray()));
        }
    }
}

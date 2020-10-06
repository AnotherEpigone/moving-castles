using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.PlayerInfo;
using MovingCastles.Maps;
using SadConsole;

namespace MovingCastles.Entities
{
    public class Castle : McEntity
    {
        public int FOVRadius;

        public Castle(Coord position, PlayerInfo playerInfo, Font font)
            : base(playerInfo.Name,
                  Color.White,
                  Color.Transparent,
                  SpriteAtlas.PlayerCastle,
                  position,
                  (int)DungeonMapLayer.PLAYER,
                  isWalkable: false,
                  isTransparent: true)
        {
            FOVRadius = 3;

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();

            AddGoRogueComponent(new InventoryComponent(playerInfo.Items.ToArray()));
        }
    }
}

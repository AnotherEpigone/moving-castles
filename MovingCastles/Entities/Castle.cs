using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Player;
using MovingCastles.Maps;
using MovingCastles.Ui;
using SadConsole;

namespace MovingCastles.Entities
{
    public class Castle : McEntity
    {
        public int FOVRadius;

        public Castle(Coord position, PlayerInfo playerInfo, Font font)
            : base("Player-Castle",
                  playerInfo.Name,
                  Color.White,
                  Color.Transparent,
                  SpriteAtlas.PlayerCastle,
                  position,
                  (int)DungeonMapLayer.PLAYER,
                  isWalkable: false,
                  isTransparent: true,
                  ColorHelper.PlayerBlue,
                  Faction.Player)
        {
            FOVRadius = 3;

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();
        }
    }
}

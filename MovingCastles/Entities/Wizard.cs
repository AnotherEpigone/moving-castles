using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Spells;
using MovingCastles.Maps;
using MovingCastles.Serialization.Entities;
using MovingCastles.Ui;
using Newtonsoft.Json;
using SadConsole;
using System.Diagnostics;

namespace MovingCastles.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(WizardJsonConverter))]
    public sealed class Wizard : McEntity
    {
        public Wizard(Coord position, PlayerInfo playerInfo, Font font)
            : base(playerInfo.Name,
                  Color.White,
                  Color.Transparent,
                  SpriteAtlas.PlayerDefault,
                  position,
                  (int)DungeonMapLayer.PLAYER,
                  isWalkable: false,
                  isTransparent: true,
                  ColorHelper.PlayerBlue)
        {
            FovRadius = 10;

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();
        }

        public int FovRadius { get; }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(Wizard)}: {Name}");
            }
        }
    }
}

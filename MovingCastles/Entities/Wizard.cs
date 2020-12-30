using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Player;
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
            : base("Player-Wizard",
                  playerInfo.Name,
                  Color.White,
                  Color.Transparent,
                  SpriteAtlas.PlayerDefault,
                  position,
                  (int)DungeonMapLayer.PLAYER,
                  isWalkable: false,
                  isTransparent: true,
                  ColorHelper.PlayerBlue,
                  Faction.Player)
        {
            FovRadius = 10;

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();
        }

        public int FovRadius { get; }

        public override string GetFlavorDescription()
        {
            if (!HasMap)
            {
                return string.Empty;
            }

            return "You are here.";
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(Wizard)}: {Name}");
            }
        }
    }
}

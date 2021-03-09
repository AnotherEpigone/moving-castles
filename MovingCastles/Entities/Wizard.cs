using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Journal;
using MovingCastles.GameSystems.Player;
using MovingCastles.Maps;
using MovingCastles.Serialization.Entities;
using MovingCastles.Ui;
using Newtonsoft.Json;
using SadConsole;
using System;
using System.Diagnostics;
using System.Linq;

namespace MovingCastles.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(WizardJsonConverter))]
    public sealed class Wizard : McEntity
    {
        public Wizard(Coord position, GameSystems.Player.WizardTemplate playerInfo, Font font)
            : this(position, playerInfo, font, Guid.NewGuid())
        { }

        public Wizard(Coord position, GameSystems.Player.WizardTemplate playerInfo, Font font, Guid id)
            : base("Player-Wizard",
                  playerInfo.Name,
                  Color.White,
                  Color.Transparent,
                  SpriteAtlas.PlayerDefault,
                  position,
                  (int)DungeonMapLayer.PLAYER,
                  isWalkable: false,
                  isTransparent: true,
                  ColorHelper.PlayerNameBlue,
                  Faction.Player,
                  id)
        {
            FovRadius = 10;

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();

            JournalEntries = playerInfo.JournalEntries
                .ToLookup(
                    je => je.TopicId,
                    je => je);
        }

        public int FovRadius { get; }

        public ILookup<string, JournalEntry> JournalEntries { get; }

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

using MovingCastles.Entities;
using Microsoft.Xna.Framework;
using GoRogue;
using MovingCastles.Serialization.Entities;
using System.Diagnostics;
using System;
using MovingCastles.Ui;
using Newtonsoft.Json;

namespace MovingCastles.GameSystems.Items
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(ItemJsonConverter))]
    public class Item : McEntity
    {
        public Item(
            string templateId,
            string name,
            int glyph,
            Color nameColor,
            System.Guid id,
            string description)
            : base(
                  templateId,
                  name,
                  Color.White,
                  Color.Transparent,
                  glyph,
                  Coord.NONE,
                  0,
                  true,
                  true,
                  nameColor,
                  Factions.Faction.None,
                  id)
        {
            Description = description;
        }

        public static Item FromTemplate(ItemTemplate template)
        {
            var item = new Item(
                template.Id,
                template.Name,
                template.Glyph,
                ColorHelper.ItemGrey,
                Guid.NewGuid(),
                template.Description);

            template.CreateComponents().ForEach(c => item.AddGoRogueComponent(c));

            return item;
        }

        public string Description { get; }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(Item)}: {Name}");
            }
        }
    }
}

using MovingCastles.Entities;
using Microsoft.Xna.Framework;
using GoRogue;
using MovingCastles.Serialization.Entities;
using System.Diagnostics;
using System;
using MovingCastles.Ui;
using Newtonsoft.Json;
using System.Text;
using MovingCastles.Components.ItemComponents;
using System.Linq;
using MovingCastles.Components.Effects;

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

        // includes component info
        public string GetFullDescription()
        {
            var descriptionBuilder = new StringBuilder(Description).AppendLine().AppendLine();
            descriptionBuilder.AppendLine($"Equip: {GetTemplate().EquipCategoryId}");
            var equippedEffects = GetGoRogueComponent<ApplyWhenEquippedComponent>()?.Components;
            if (equippedEffects != null)
            {
                foreach (var effect in equippedEffects.OfType<IDescribableEffect>())
                {
                    descriptionBuilder.AppendLine(effect.GetDescription());
                }
            }

            return descriptionBuilder.ToString();
        }

        public ItemTemplate GetTemplate() => ItemAtlas.ItemsById[TemplateId];

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(Item)}: {Name}");
            }
        }
    }
}

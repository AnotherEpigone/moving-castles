using MovingCastles.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MovingCastles.Components.Serialization;
using MovingCastles.Fonts;
using MovingCastles.Components.ItemComponents;
using MovingCastles.Entities;

namespace MovingCastles.GameSystems.Items
{
    public static class ItemAtlas
    {
        static ItemAtlas()
        {
            ItemsById = typeof(ItemAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<ItemTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i);
        }

        public static Dictionary<string, ItemTemplate> ItemsById { get; }

        public static ItemTemplate SteelLongsword => new ItemTemplate(
            id: "ITEM_STEEL_LONGSWORD",
            name: "Steel longsword",
            description: Gui.Item_SteelLongsword_Desc,
            glyph: 11,
            createComponents: () => new List<ISerializableComponent>());
        public static ItemTemplate EtheriumShard => new ItemTemplate(
            id: "ITEM_ETHERIUM_SHARD",
            name: "Etherium shard",
            description: Gui.Item_EtheriumShard_Desc,
            glyph: 10,
            createComponents: () => new List<ISerializableComponent>());
        public static ItemTemplate StarterOakStaff => new ItemTemplate(
            id: "ITEM_STARTER_OAKSTAFF",
            name: "Oak staff",
            description: Gui.Item_StarterOakStaff_Desc,
            glyph: 11,
            createComponents: () => new List<ISerializableComponent>());
        public static ItemTemplate TrollShroom => new ItemTemplate(
            id: "ITEM_TROLLSHROOM",
            name: "Trollshroom",
            description: "Todo trollshroom description",
            glyph: DungeonModeSpriteAtlas.Trollshroom_Small,
            createComponents: () => new List<ISerializableComponent>
            {
                new ApplyInInventoryEffectsComponent(new List<ISerializableComponent>
                {
                    new SpawnActorAtParentComponent(ActorAtlas.Troll.Id, 1000),
                }),
            });
    }
}

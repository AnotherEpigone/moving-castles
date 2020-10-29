using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            description: "The nobleman's weapon of war, with a well-used leather grip and a blade of the finest Ayeni steel.",
            glyph: 11);
        public static ItemTemplate EtheriumShard => new ItemTemplate(
            id: "ITEM_ETHERIUM_SHARD",
            name: "Etherium shard",
            description: "Crystalized by the precise artistry of master artificers, etherium is the closest thing you can get to the pure substance of the Torrent.",
            glyph: 10);
        public static ItemTemplate StarterOakStaff => new ItemTemplate(
            id: "ITEM_STARTER_OAKSTAFF",
            name: "Oak staff",
            description: "Cut from the woods of the Academy at Kurisau, this staff has served you since you first learned to sense the Wellspring.",
            glyph: 11);
    }
}

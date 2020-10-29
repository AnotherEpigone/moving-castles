using System.Collections.Generic;

namespace MovingCastles.Maps
{
    public static class MapAtlas
    {
        public static MapTemplate CombatTestArea => new MapTemplate(
            id: "MAP_TESTAREA",
            floorItems: new List<string>
            {
                "ITEM_STEEL_LONGSWORD",
                "ITEM_ETHERIUM_SHARD",
            });
    }
}

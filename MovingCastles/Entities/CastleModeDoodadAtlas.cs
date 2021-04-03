using MovingCastles.Fonts;
using MovingCastles.Ui;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovingCastles.Entities
{
    public static class CastleModeDoodadAtlas
    {
        static CastleModeDoodadAtlas()
        {
            ById = typeof(DungeonModeDoodadAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<DoodadTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i);
        }

        public static Dictionary<string, DoodadTemplate> ById { get; }

        public static DoodadTemplate AlwardsTower => new DoodadTemplate(
            id: "DOODAD_ALWARDS",
            name: "Old Alward's Tower",
            glyph: DungeonModeSpriteAtlas.PlayerCastle,
            nameColor: ColorHelper.ItemGrey,
            walkable: true,
            transparent: true);
    }
}

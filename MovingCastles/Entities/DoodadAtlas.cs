using Microsoft.Xna.Framework;
using MovingCastles.Fonts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovingCastles.Entities
{
    public static class DoodadAtlas
    {
        static DoodadAtlas()
        {
            ActorsById = typeof(DoodadAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<DoodadTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i);
        }

        public static Dictionary<string, DoodadTemplate> ActorsById { get; }

        public static DoodadTemplate Trapdoor => new DoodadTemplate(
            id: "DOODAD_TRAPDOOR_WOOD",
            name: "Wooden trapdoor",
            glyph: SpriteAtlas.Trapdoor_Wood,
            nameColor: Color.LightGray,
            walkable: true,
            transparent: true);

        public static DoodadTemplate StaircaseUp => new DoodadTemplate(
            id: "DOODAD_STAIRCASE_UP",
            name: "Staircase up",
            glyph: SpriteAtlas.Staircase_Up,
            nameColor: Color.LightGray,
            walkable: true,
            transparent: true);

        public static DoodadTemplate EtheriumCoreWithStand => new DoodadTemplate(
            id: "DOODAD_ETHERIUMCORE_STAND",
            name: "Etherium core",
            glyph: SpriteAtlas.EtheriumCore_Stand,
            nameColor: Color.LightGray,
            walkable: true,
            transparent: true);
    }
}

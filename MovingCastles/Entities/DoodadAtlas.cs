using Microsoft.Xna.Framework;
using MovingCastles.Fonts;
using MovingCastles.Ui;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovingCastles.Entities
{
    public static class DoodadAtlas
    {
        static DoodadAtlas()
        {
            ById = typeof(DoodadAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<DoodadTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i);
        }

        public static Dictionary<string, DoodadTemplate> ById { get; }

        public static DoodadTemplate Trapdoor => new DoodadTemplate(
            id: "DOODAD_TRAPDOOR_WOOD",
            name: "Wooden trapdoor",
            glyph: SpriteAtlas.Trapdoor_Wood,
            nameColor: Color.SaddleBrown,
            walkable: true,
            transparent: true);

        public static DoodadTemplate StaircaseUp => new DoodadTemplate(
            id: "DOODAD_STAIRCASE_UP",
            name: "Staircase up",
            glyph: SpriteAtlas.Staircase_Up,
            nameColor: Color.SaddleBrown,
            walkable: true,
            transparent: true);

        public static DoodadTemplate StaircaseDown => new DoodadTemplate(
            id: "DOODAD_STAIRCASE_Down",
            name: "Staircase down",
            glyph: SpriteAtlas.Staircase_Down,
            nameColor: Color.SaddleBrown,
            walkable: true,
            transparent: true);

        public static DoodadTemplate EtheriumCoreWithStand => new DoodadTemplate(
            id: "DOODAD_ETHERIUMCORE_STAND",
            name: "Etherium core",
            glyph: SpriteAtlas.EtheriumCore_Stand,
            nameColor: Color.Violet,
            walkable: true,
            transparent: true);

        public static DoodadTemplate StoneRubble => new DoodadTemplate(
            id: "DOODAD_STONE_RUBBLE",
            name: "Stone rubble",
            glyph: SpriteAtlas.StoneRubble,
            nameColor: ColorHelper.ItemGrey,
            walkable: true,
            transparent: true);
    }
}

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
            id: "DOODAD_STAIRCASE_DOWN",
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
            id: "DOODAD_STONERUBBLE",
            name: "Stone rubble",
            glyph: SpriteAtlas.StoneRubble,
            nameColor: ColorHelper.ItemGrey,
            walkable: true,
            transparent: true);

        public static DoodadTemplate HeavyStoneRubble => new DoodadTemplate(
            id: "DOODAD_STONERUBBLE_HEAVY",
            name: "Heavy stone rubble",
            glyph: SpriteAtlas.HeavyStoneRubble,
            nameColor: ColorHelper.ItemGrey,
            walkable: false,
            transparent: true);

        public static DoodadTemplate SmallBookshelf => new DoodadTemplate(
            id: "DOODAD_BOOKSHELF_SMALL",
            name: "Bookshelf",
            glyph: SpriteAtlas.Bookshelf_Small,
            nameColor: Color.SaddleBrown,
            walkable: false,
            transparent: true);

        public static DoodadTemplate SmallDesk => new DoodadTemplate(
            id: "DOODAD_DESK_SMALL",
            name: "Desk",
            glyph: SpriteAtlas.Desk_Small,
            nameColor: Color.SaddleBrown,
            walkable: false,
            transparent: true);
    }
}

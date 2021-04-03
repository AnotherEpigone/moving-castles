using Microsoft.Xna.Framework;
using MovingCastles.Fonts;
using MovingCastles.Ui;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovingCastles.Entities
{
    public static class DungeonModeDoodadAtlas
    {
        static DungeonModeDoodadAtlas()
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

        public static DoodadTemplate BandedWoodenDoor => new DoodadTemplate(
            id: "DOODAD_BANDED_WOODEN_DOOR",
            name: "Iron-banded wooden door",
            glyph: DungeonModeSpriteAtlas.Door_Banded_Closed,
            nameColor: Color.SaddleBrown,
            walkable: false,
            transparent: false);

        public static DoodadTemplate Trapdoor => new DoodadTemplate(
            id: "DOODAD_TRAPDOOR_WOOD",
            name: "Wooden trapdoor",
            glyph: DungeonModeSpriteAtlas.Trapdoor_Wood,
            nameColor: Color.SaddleBrown,
            walkable: true,
            transparent: true);

        public static DoodadTemplate StaircaseUp => new DoodadTemplate(
            id: "DOODAD_STAIRCASE_UP",
            name: "Staircase up",
            glyph: DungeonModeSpriteAtlas.Staircase_Up,
            nameColor: Color.SaddleBrown,
            walkable: true,
            transparent: true);

        public static DoodadTemplate StaircaseDown => new DoodadTemplate(
            id: "DOODAD_STAIRCASE_DOWN",
            name: "Staircase down",
            glyph: DungeonModeSpriteAtlas.Staircase_Down,
            nameColor: Color.SaddleBrown,
            walkable: true,
            transparent: true);

        public static DoodadTemplate EtheriumCoreWithStand => new DoodadTemplate(
            id: "DOODAD_ETHERIUMCORE_STAND",
            name: "Etherium core",
            glyph: DungeonModeSpriteAtlas.EtheriumCore_Stand,
            nameColor: Color.Violet,
            walkable: true,
            transparent: true);

        public static DoodadTemplate StoneRubble => new DoodadTemplate(
            id: "DOODAD_STONERUBBLE",
            name: "Stone rubble",
            glyph: DungeonModeSpriteAtlas.StoneRubble,
            nameColor: ColorHelper.ItemGrey,
            walkable: true,
            transparent: true);

        public static DoodadTemplate HeavyStoneRubble => new DoodadTemplate(
            id: "DOODAD_STONERUBBLE_HEAVY",
            name: "Heavy stone rubble",
            glyph: DungeonModeSpriteAtlas.HeavyStoneRubble,
            nameColor: ColorHelper.ItemGrey,
            walkable: false,
            transparent: true);

        public static DoodadTemplate SmallBookshelf => new DoodadTemplate(
            id: "DOODAD_BOOKSHELF_SMALL",
            name: "Bookshelf",
            glyph: DungeonModeSpriteAtlas.Bookshelf_Small,
            nameColor: Color.SaddleBrown,
            walkable: false,
            transparent: true);

        public static DoodadTemplate SmallDesk => new DoodadTemplate(
            id: "DOODAD_DESK_SMALL",
            name: "Desk",
            glyph: DungeonModeSpriteAtlas.Desk_Small,
            nameColor: Color.SaddleBrown,
            walkable: false,
            transparent: true);

        public static DoodadTemplate SmallBarrel => new DoodadTemplate(
            id: "DOODAD_BARREL_SMALL",
            name: "Barrel",
            glyph: DungeonModeSpriteAtlas.Barrel_Small,
            nameColor: Color.SaddleBrown,
            walkable: false,
            transparent: true);

        public static DoodadTemplate SmallChest => new DoodadTemplate(
            id: "DOODAD_CHEST_SMALL",
            name: "Chest",
            glyph: DungeonModeSpriteAtlas.Chest_Small,
            nameColor: Color.SaddleBrown,
            walkable: false,
            transparent: true);
    }
}

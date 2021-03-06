﻿using MovingCastles.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MovingCastles.Components.Serialization;
using MovingCastles.Fonts;
using MovingCastles.Components.ItemComponents;
using MovingCastles.Entities;
using MovingCastles.Components.Effects;

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
            glyph: DungeonModeSpriteAtlas.SteelLongsword,
            equipCategoryId: EquipCategoryId.Weapon,
            createComponents: () => new List<ISerializableComponent>
            {
                new ApplyWhenEquippedComponent(new List<ISerializableComponent>
                {
                    new EquippedMeleeWeaponComponent("1d6+2", 0),
                }),
            });
        public static ItemTemplate EtheriumShard => new ItemTemplate(
            id: "ITEM_ETHERIUM_SHARD",
            name: "Etherium shard",
            description: Gui.Item_EtheriumShard_Desc,
            glyph: DungeonModeSpriteAtlas.EtheriumShard,
            equipCategoryId: EquipCategoryId.Trinket,
            createComponents: () => new List<ISerializableComponent>
            {
                new ApplyWhenEquippedComponent(new List<ISerializableComponent>
                {
                    new HealthRegenEffect(0.1f),
                    new EndowmentRegenEffect(1),
                })
            });
        public static ItemTemplate StarterOakStaff => new ItemTemplate(
            id: "ITEM_STARTER_OAKSTAFF",
            name: "Oak staff",
            description: Gui.Item_StarterOakStaff_Desc,
            glyph: DungeonModeSpriteAtlas.Staff_Oak,
            equipCategoryId: EquipCategoryId.Staff,
            createComponents: () => new List<ISerializableComponent>
            {
                new ApplyWhenEquippedComponent(new List<ISerializableComponent>
                {
                    new FovRangeEffect(3),
                }),
            });
        public static ItemTemplate StarterHomespunCloak => new ItemTemplate(
            id: "ITEM_STARTER_HOMESPUN_CLOAK",
            name: "Homespun cloak",
            description: Gui.Item_HomespunCloak_Desc,
            glyph: DungeonModeSpriteAtlas.Cloak_Homespun,
            equipCategoryId: EquipCategoryId.Cloak,
            createComponents: () => new List<ISerializableComponent>
            {
                new ApplyWhenEquippedComponent(new List<ISerializableComponent>
                {
                    new DeflectEffect(30),
                })
            });
        public static ItemTemplate TrollShroom => new ItemTemplate(
            id: "ITEM_TROLLSHROOM",
            name: "Trollshroom",
            description: Gui.Item_TrollShroom_Desc,
            glyph: DungeonModeSpriteAtlas.Trollshroom_Small,
            equipCategoryId: EquipCategoryId.None,
            createComponents: () => new List<ISerializableComponent>
            {
                new ApplyInInventoryEffectsComponent(new List<ISerializableComponent>
                {
                    new SpawnActorAtParentComponent(ActorAtlas.Troll.Id, 1000),
                }),
            });
    }
}

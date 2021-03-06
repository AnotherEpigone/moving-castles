﻿using MovingCastles.Components.Serialization;
using System;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Items
{
    public class ItemTemplate
    {
        public ItemTemplate(
            string id,
            string name,
            string description,
            int glyph,
            EquipCategoryId equipCategoryId,
            Func<List<ISerializableComponent>> createComponents)
        {
            Id = id;
            Name = name;
            Description = description;
            Glyph = glyph;
            CreateComponents = createComponents;
            EquipCategoryId = equipCategoryId;
        }

        public string Id { get; }

        public string Name { get; }

        public string Description { get; }

        public int Glyph { get; }

        public EquipCategoryId EquipCategoryId { get; }

        public Func<List<ISerializableComponent>> CreateComponents { get; }
    }
}

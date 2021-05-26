using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using System;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public interface IEquipmentComponent : ISerializableComponent
    {
        event EventHandler EquipmentChanged;

        IReadOnlyDictionary<EquipCategoryId, EquipCategory> Equipment { get; }

        bool CanEquip(Item item, EquipCategoryId categoryId);

        bool Equip(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster, ILogManager logManager);

        bool Unequip(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster, ILogManager logManager);
    }
}

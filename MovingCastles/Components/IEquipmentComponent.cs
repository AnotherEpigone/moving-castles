using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public interface IEquipmentComponent : ISerializableComponent
    {
        IReadOnlyDictionary<EquipCategoryId, EquipCategory> Equipment { get; }

        bool Equip(Item item, EquipCategoryId categoryId, ILogManager logManager);

        bool Unequip(Item item, EquipCategoryId categoryId, ILogManager logManager);
    }
}

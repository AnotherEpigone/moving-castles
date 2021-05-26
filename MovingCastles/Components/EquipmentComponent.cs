using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Components
{
    public class EquipmentComponent : IEquipmentComponent
    {
        private Dictionary<EquipCategoryId, EquipCategory> _equipCategories;

        public EquipmentComponent(EquipCategory[] categories)
        {
            _equipCategories = categories.ToDictionary(
                c => c.Id,
                c => c);
        }

        public EquipmentComponent(SerializedObject state)
            : this(JsonConvert.DeserializeObject<EquipCategory[]>(state.Value)) { }

        public event System.EventHandler EquipmentChanged;

        public IGameObject Parent { get; set; }

        public IReadOnlyDictionary<EquipCategoryId, EquipCategory> Equipment => _equipCategories;

        public bool CanEquip(Item item, EquipCategoryId categoryId)
        {
            return _equipCategories.TryGetValue(categoryId, out var category)
                && category.Slots - category.Items.Count >= 1;
        }

        public bool Equip(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            if (!_equipCategories.TryGetValue(categoryId, out var category))
            {
                return false;
            }

            if (category.Slots - category.Items.Count < 1)
            {
                return false;
            }

            logManager.StoryLog($"Equipped {item.ColoredName}.");
            category.Items.Add(item);

            EquipmentChanged?.Invoke(this, EventArgs.Empty);
            foreach (var triggeredComponent in item.GetGoRogueComponents<IEquipTriggeredComponent>())
            {
                triggeredComponent.OnEquip((McEntity)Parent, dungeonMaster, logManager);
            }

            return true;
        }

        public bool Unequip(Item item, EquipCategoryId categoryId, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            if (!_equipCategories.TryGetValue(categoryId, out var category))
            {
                return false;
            }

            var success = category.Items.Remove(item);
            if (success)
            {
                logManager.StoryLog($"Unequipped {item.ColoredName}.");
            }

            EquipmentChanged?.Invoke(this, EventArgs.Empty);
            foreach (var triggeredComponent in item.GetGoRogueComponents<IEquipTriggeredComponent>())
            {
                triggeredComponent.OnUnequip((McEntity)Parent, dungeonMaster, logManager);
            }

            return success;
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(EquipmentComponent),
                State = JsonConvert.SerializeObject(_equipCategories.Values.ToArray()),
            };
        }
    }
}

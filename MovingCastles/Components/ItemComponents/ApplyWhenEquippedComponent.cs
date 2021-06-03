using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Components.ItemComponents
{
    public class ApplyWhenEquippedComponent : IEquipTriggeredComponent
    {
        public ApplyWhenEquippedComponent(IEnumerable<ISerializableComponent> components)
        {
            Components = components;
        }

        public ApplyWhenEquippedComponent(SerializedObject serialized)
            : this(JsonConvert.DeserializeObject<List<ComponentSerializable>>(serialized.Value).Select(cs => ComponentFactory.Create(cs)))
        {
        }

        public IGameObject Parent { get; set; }

        public IEnumerable<ISerializableComponent> Components { get; }

        public void OnEquip(McEntity equipmentOwner, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            foreach (var component in Components)
            {
                equipmentOwner.AddGoRogueComponent(component);
            }
        }

        public void OnUnequip(McEntity equipmentOwner, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            foreach (var component in Components)
            {
                equipmentOwner.RemoveGoRogueComponent(component);
            }
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(ApplyWhenEquippedComponent),
            State = JsonConvert.SerializeObject(Components.Select(c => c.GetSerializable()).ToList()),
        };
    }
}

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
    public class ApplyInInventoryEffectsComponent : IInventoryTriggeredComponent
    {
        private readonly IEnumerable<ISerializableComponent> _components;

        public ApplyInInventoryEffectsComponent(SerializedObject serialized)
            : this(JsonConvert.DeserializeObject<List<ComponentSerializable>>(serialized.Value).Select(cs => ComponentFactory.Create(cs)))
        {
        }

        public ApplyInInventoryEffectsComponent(IEnumerable<ISerializableComponent> components)
        {
            _components = components;
        }

        public IGameObject Parent { get; set; }

        public void OnAddedToInventory(McEntity inventoryOwner, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            foreach (var component in _components)
            {
                inventoryOwner.AddGoRogueComponent(component);
            }
        }

        public void OnRemovedFromInventory(McEntity inventoryOwner, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            foreach (var component in _components)
            {
                inventoryOwner.RemoveGoRogueComponent(component);
            }
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(ApplyInInventoryEffectsComponent),
            State = JsonConvert.SerializeObject(_components.Select(c => c.GetSerializable()).ToList()),
        };
    }
}

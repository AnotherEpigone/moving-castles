using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Components
{
    public class InventoryComponent : IGameObjectComponent, IInventoryComponent, ISerializableComponent
    {
        private readonly List<Item> _items;

        public InventoryComponent(params Item[] items)
        {
            _items = items.ToList();
        }

        public InventoryComponent(SerializedObject state)
            : this(JsonConvert.DeserializeObject<List<Item>>(state.Value).ToArray()) { }

        public IGameObject Parent { get; set; }

        public void AddItem(Item item, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            _items.Add(item);
            foreach (var triggeredComponent in item.GetGoRogueComponents<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnAddedToInventory((McEntity)Parent, dungeonMaster, logManager);
            }
        }

        public void RemoveItem(Item item, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            _items.Remove(item);
            foreach (var triggeredComponent in item.GetGoRogueComponents<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnRemovedFromInventory((McEntity)Parent, dungeonMaster, logManager);
            }
        }

        public IReadOnlyCollection<Item> GetItems()
        {
            return _items;
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(InventoryComponent),
                State = JsonConvert.SerializeObject(_items),
            };
        }
    }
}

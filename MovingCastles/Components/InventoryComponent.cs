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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Components
{
    public class InventoryComponent : IGameObjectComponent, IInventoryComponent
    {
        private readonly List<Item> _items;

        public InventoryComponent(int capacity, params Item[] items)
        {
            _items = items.ToList();
            Capacity = capacity;
        }

        public InventoryComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _items = stateObj.Items;
            Capacity = stateObj.Capacity;
        }

        public event EventHandler ContentsChanged;

        public IGameObject Parent { get; set; }

        public int FilledCapacity => _items.Count;

        public int Capacity { get; }

        public void AddItem(Item item, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            _items.Add(item);
            foreach (var triggeredComponent in item.GetGoRogueComponents<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnAddedToInventory((McEntity)Parent, dungeonMaster, logManager);
            }

            ContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(Item item, IDungeonMaster dungeonMaster, ILogManager logManager)
        {
            _items.Remove(item);
            foreach (var triggeredComponent in item.GetGoRogueComponents<IInventoryTriggeredComponent>())
            {
                triggeredComponent.OnRemovedFromInventory((McEntity)Parent, dungeonMaster, logManager);
            }

            ContentsChanged?.Invoke(this, EventArgs.Empty);
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
                State = JsonConvert.SerializeObject(new State
                {
                    Capacity = Capacity,
                    Items = _items,
                }),
            };
        }

        [DataContract]
        private class State
        {
            [DataMember] public int Capacity;
            [DataMember] public List<Item> Items;
        }
    }
}

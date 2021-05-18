using GoRogue.GameFramework;
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
using Troschuetz.Random;

namespace MovingCastles.Components
{
    public class PickupItemComponent : ISerializableComponent, IStepTriggeredComponent
    {
        public PickupItemComponent(params Item[] items)
        {
            Items = new List<Item>(items);
        }

        public PickupItemComponent(SerializedObject state)
            : this(JsonConvert.DeserializeObject<List<Item>>(state.Value).ToArray()) { }

        public List<Item> Items { get; }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster dungeonMaster, IGenerator rng)
        {
            var inventory = steppingEntity.GetGoRogueComponent<IInventoryComponent>();
            if (inventory == null)
            {
                return;
            }

            if (inventory.Capacity - inventory.FilledCapacity < Items.Count)
            {
                logManager.CombatLog($"{steppingEntity.ColoredName} can't pick up {string.Join(", ", Items.Select(i => i.ColoredName))}. Inventory full.");
                return;
            }

            foreach (var item in Items)
            {
                inventory.AddItem(item, dungeonMaster, logManager);
            }

            Parent.CurrentMap.RemoveEntity(Parent);

            logManager.StoryLog($"{steppingEntity.ColoredName} picked up {string.Join(", ", Items.Select(i => i.ColoredName))}.");
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(PickupItemComponent),
                State = JsonConvert.SerializeObject(Items),
            };
        }
    }
}

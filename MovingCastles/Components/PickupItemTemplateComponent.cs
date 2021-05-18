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
    public class PickupItemTemplateComponent : ISerializableComponent, IStepTriggeredComponent
    {
        public PickupItemTemplateComponent(params ItemTemplate[] items)
        {
            Items = new List<ItemTemplate>(items);
        }

        public PickupItemTemplateComponent(SerializedObject state)
            : this(JsonConvert.DeserializeObject<string[]>(state.Value).Select(id => ItemAtlas.ItemsById[id]).ToArray()) { }

        public List<ItemTemplate> Items { get; }

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
                logManager.CombatLog($"{steppingEntity.ColoredName} can't pick up {string.Join(", ", Items.Select(i => Item.FromTemplate(i).ColoredName))}. Inventory full.");
                return;
            }

            foreach (var item in Items)
            {
                inventory.AddItem(Item.FromTemplate(item), dungeonMaster, logManager);
            }

            Parent.CurrentMap.RemoveEntity(Parent);

            logManager.StoryLog($"{steppingEntity.ColoredName} picked up {string.Join(", ", Items.Select(i => Item.FromTemplate(i).ColoredName))}.");
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(PickupItemTemplateComponent),
                State = JsonConvert.SerializeObject(Items.Select(i => i.Id).ToArray()),
            };
        }
    }
}

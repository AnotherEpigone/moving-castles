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

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster gameManager, IGenerator rng)
        {
            var inventory = steppingEntity.GetGoRogueComponent<IInventoryComponent>();
            if (inventory == null)
            {
                return;
            }

            inventory.Items.AddRange(Items);
            Parent.CurrentMap.RemoveEntity(Parent);

            logManager.StoryLog($"{steppingEntity.ColoredName} picked up {string.Join(", ", Items.Select(i => i.Name))}.");
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

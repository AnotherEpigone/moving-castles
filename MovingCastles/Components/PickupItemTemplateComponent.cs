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
            : this(JsonConvert.DeserializeObject<List<ItemTemplate>>(state.Value).ToArray()) { }

        public List<ItemTemplate> Items { get; }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster gameManager, IGenerator rng)
        {
            var inventory = steppingEntity.GetGoRogueComponent<IInventoryComponent>();
            if (inventory == null)
            {
                return;
            }

            inventory.Items.AddRange(Items.Select(i => Item.FromTemplate(i)));
            Parent.CurrentMap.RemoveEntity(Parent);

            logManager.StoryLog($"{steppingEntity.ColoredName} picked up {string.Join(", ", Items.Select(i => i.Name))}.");
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(PickupItemTemplateComponent),
                State = JsonConvert.SerializeObject(Items),
            };
        }
    }
}

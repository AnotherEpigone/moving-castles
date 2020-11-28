using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
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
    public class PickupableComponent : IGameObjectComponent, IPickupableComponent, ISerializableComponent
    {
        public PickupableComponent(params ItemTemplate[] items)
        {
            Items = new List<ItemTemplate>(items);
        }

        public PickupableComponent(SerializedObject state)
            : this(JsonConvert.DeserializeObject<List<ItemTemplate>>(state.Value).ToArray()) { }

        public List<ItemTemplate> Items { get; }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IGameManager gameManager)
        {
            var inventory = steppingEntity.GetGoRogueComponent<IInventoryComponent>();
            if (inventory == null)
            {
                return;
            }

            inventory.Items.AddRange(Items);
            Parent.CurrentMap.RemoveEntity(Parent);

            logManager.EventLog($"{steppingEntity.ColoredName} picked up {string.Join(", ", Items.Select(i => i.Name))}.");
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(PickupableComponent),
                State = JsonConvert.SerializeObject(Items),
            };
        }
    }
}

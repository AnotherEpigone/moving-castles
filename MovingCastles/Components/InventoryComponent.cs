using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems.Items;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Components
{
    public class InventoryComponent : IGameObjectComponent, IInventoryComponent, ISerializableComponent
    {
        public InventoryComponent(params Item[] items)
        {
            Items = items.ToList();
        }

        public InventoryComponent(SerializedObject state)
            : this(JsonConvert.DeserializeObject<List<Item>>(state.Value).ToArray()) { }

        public IGameObject Parent { get; set; }

        public List<Item> Items { get; }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(InventoryComponent),
                State = JsonConvert.SerializeObject(Items),
            };
        }
    }
}

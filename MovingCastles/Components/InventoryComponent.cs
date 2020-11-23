using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems.Items;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public class InventoryComponent : IGameObjectComponent, IInventoryComponent, ISerializableComponent
    {
        public InventoryComponent(params ItemTemplate[] items)
        {
            Items = new List<ItemTemplate>(items);
        }

        public InventoryComponent(SerializedObject state)
            : this(JsonConvert.DeserializeObject<List<ItemTemplate>>(state.Value).ToArray()) { }

        public IGameObject Parent { get; set; }

        public List<ItemTemplate> Items { get; }

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

using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public class InventoryComponent : IGameObjectComponent, IInventoryComponent
    {
        public InventoryComponent(params ItemTemplate[] items)
        {
            Items = new List<ItemTemplate>(items);
        }

        public IGameObject Parent { get; set; }

        public List<ItemTemplate> Items { get; }
    }
}

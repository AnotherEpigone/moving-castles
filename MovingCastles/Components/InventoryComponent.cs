using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public class InventoryComponent : IGameObjectComponent, IInventoryComponent
    {
        public InventoryComponent()
        {
            Items = new List<IInventoryItem>();
        }

        public IGameObject Parent { get; set; }

        public List<IInventoryItem> Items { get; }
    }
}

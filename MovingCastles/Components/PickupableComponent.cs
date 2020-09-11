using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public class PickupableComponent : IGameObjectComponent, IPickupableComponent
    {
        public PickupableComponent(params IInventoryItem[] items)
        {
            Items = new List<IInventoryItem>(items);
        }

        public List<IInventoryItem> Items { get; }

        public IGameObject Parent { get; set; }

        public void OnStep(IHasComponents steppingEntity)
        {
            var inventory = steppingEntity.GetComponent<IInventoryComponent>();
            if (inventory == null)
            {
                return;
            }

            inventory.Items.AddRange(Items);
            Parent.CurrentMap.RemoveEntity(Parent);
        }
    }
}

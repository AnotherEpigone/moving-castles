using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public interface IInventoryComponent : ISerializableComponent
    {
        IReadOnlyCollection<Item> GetItems();

        void AddItem(Item item, IDungeonMaster dungeonMaster, ILogManager logManager);

        void RemoveItem(Item item, IDungeonMaster dungeonMaster, ILogManager logManager);
    }
}
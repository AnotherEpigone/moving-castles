using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using System;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public interface IInventoryComponent : ISerializableComponent
    {
        event EventHandler ContentsChanged;

        public int FilledCapacity { get; }

        public int Capacity { get; }

        IReadOnlyCollection<Item> GetItems();

        void AddItem(Item item, IDungeonMaster dungeonMaster, ILogManager logManager);

        void RemoveItem(Item item, IDungeonMaster dungeonMaster, ILogManager logManager);
    }
}
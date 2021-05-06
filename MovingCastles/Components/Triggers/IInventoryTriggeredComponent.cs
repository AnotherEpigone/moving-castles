using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components.Triggers
{
    public interface IInventoryTriggeredComponent : ISerializableComponent
    {
        public void OnAddedToInventory(McEntity inventoryOwner, IDungeonMaster dungeonMaster, ILogManager logManager);

        public void OnRemovedFromInventory(McEntity inventoryOwner, IDungeonMaster dungeonMaster, ILogManager logManager);
    }
}

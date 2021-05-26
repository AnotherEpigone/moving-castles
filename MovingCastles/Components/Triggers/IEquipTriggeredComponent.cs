using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components.Triggers
{
    interface IEquipTriggeredComponent : ISerializableComponent
    {
        public void OnEquip(McEntity equipmentOwner, IDungeonMaster dungeonMaster, ILogManager logManager);

        public void OnUnequip(McEntity equipmentOwner, IDungeonMaster dungeonMaster, ILogManager logManager);
    }
}

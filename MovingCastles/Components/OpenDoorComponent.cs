using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components
{
    public class OpenDoorComponent : IBumpTriggeredComponent, ISerializableComponent
    {
        public IGameObject Parent { get; set; }

        public void Bump(McEntity bumpingEntity, ILogManager logManager, IDungeonMaster dungeonMaster)
        {
            if (Parent is not Door door)
            {
                return;
            }

            door.Open();
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(OpenDoorComponent),
            };
        }
    }
}

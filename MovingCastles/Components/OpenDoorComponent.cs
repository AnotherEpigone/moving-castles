using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;

namespace MovingCastles.Components
{
    public class OpenDoorComponent : IBumpTriggeredComponent, ISerializableComponent
    {
        public IGameObject Parent { get; set; }

        public void Bump(McEntity bumpingEntity)
        {
            if (!(Parent is Door door))
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

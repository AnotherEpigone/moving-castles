using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Time
{
    [DataContract]
    public class EntityTurnTimeMasterNode : ITimeMasterNode
    {
        public EntityTurnTimeMasterNode(long time, System.Guid entityId)
        {
            Time = time;
            EntityId = entityId;
        }

        [DataMember]
        public long Time { get; init; }

        [DataMember]
        public System.Guid EntityId { get; init; }
    }
}

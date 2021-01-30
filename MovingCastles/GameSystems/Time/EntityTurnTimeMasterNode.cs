using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Time
{
    [DataContract]
    public class EntityTurnTimeMasterNode : ITimeMasterNode
    {
        public EntityTurnTimeMasterNode(long time, uint entityId)
        {
            Time = time;
            EntityId = entityId;
        }

        [DataMember]
        public long Time { get; init; }

        [DataMember]
        public uint EntityId { get; init; }
    }
}

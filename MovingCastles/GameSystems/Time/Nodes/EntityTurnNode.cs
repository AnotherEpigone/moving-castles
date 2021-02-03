using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Time.Nodes
{
    [DataContract]
    public class EntityTurnNode : ITimeMasterNode
    {
        public EntityTurnNode(long time, System.Guid entityId)
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

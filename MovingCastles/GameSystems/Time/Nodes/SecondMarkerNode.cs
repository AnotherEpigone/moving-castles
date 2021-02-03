using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Time.Nodes
{
    [DataContract]
    public class SecondMarkerNode : ITimeMasterNode
    {
        public SecondMarkerNode(long time)
        {
            Time = time;
        }

        [DataMember]
        public long Time { get; }
    }
}

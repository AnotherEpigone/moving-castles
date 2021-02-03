using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Time.Nodes
{
    [DataContract]
    public class WizardTurnNode : ITimeMasterNode
    {
        public WizardTurnNode(long time)
        {
            Time = time;
        }

        [DataMember]
        public long Time { get; }
    }
}

using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Time
{
    [DataContract]
    public class WizardTurnTimeMasterNode : ITimeMasterNode
    {
        public WizardTurnTimeMasterNode(long time)
        {
            Time = time;
        }

        [DataMember]
        public long Time { get; }
    }
}

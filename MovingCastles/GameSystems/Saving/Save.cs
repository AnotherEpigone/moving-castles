using MovingCastles.Entities;
using MovingCastles.GameSystems.Time;
using MovingCastles.Serialization.Map;
using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Saving
{
    [DataContract]
    public class Save
    {
        [DataMember] public MapState[] KnownMaps;
        [DataMember] public MapState MapState;
        [DataMember] public Wizard Wizard;
        [DataMember] public TimeMaster TimeMaster;
        [DataMember] public GameMode GameMode;
    }
}

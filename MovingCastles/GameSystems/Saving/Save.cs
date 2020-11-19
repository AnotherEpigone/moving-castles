using MovingCastles.Entities;
using MovingCastles.Serialization.Map;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Saving
{
    [DataContract]
    public class Save
    {
        [DataMember] public MapState MapState;
        [DataMember] public List<McEntity> Entities;
        [DataMember] public List<Door> Doors;
        [DataMember] public Wizard Wizard;
    }
}

using MovingCastles.Entities;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Saving
{
    [DataContract]
    public class Save
    {
        [DataMember] public int Seed;
        [DataMember] public List<McEntity> Entities;
        [DataMember] public Wizard Wizard;
    }
}

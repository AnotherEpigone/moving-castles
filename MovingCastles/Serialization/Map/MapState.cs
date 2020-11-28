using MovingCastles.Entities;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MovingCastles.Serialization.Map
{
    /// <summary>
    /// All the information needed to recreate a DungeonMap
    /// </summary>
    [DataContract]
    public class MapState
    {
        [DataMember] public string Id;
        [DataMember] public int Seed;
        [DataMember] public int Height;
        [DataMember] public int Width;
        [DataMember] public bool[] Explored;
        [DataMember] public List<McEntity> Entities;
        [DataMember] public List<Door> Doors;
        [DataMember] public string StructureId;
    }
}

using System.Runtime.Serialization;

namespace MovingCastles.Serialization.Map
{
    /// <summary>
    /// All the information needed to recreate a DungeonMap
    /// </summary>
    [DataContract]
    public class MapState
    {
        [DataMember] public int Seed;
        [DataMember] public int Height;
        [DataMember] public int Width;
        [DataMember] public bool[] Explored;
    }
}

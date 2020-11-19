using System.Runtime.Serialization;

namespace MovingCastles.Components.Serialization
{
    [DataContract]
    public class ComponentSerializable
    {
        [DataMember] public string Id;
        [DataMember] public string State;
    }
}

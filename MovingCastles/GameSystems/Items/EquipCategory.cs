using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Items
{
    public enum EquipCategoryId
    {
        None,
        Staff,
        Cloak,
        Weapon,
    }

    [DataContract]
    public class EquipCategory
    {
        [DataMember]
        public EquipCategoryId Id { get; init; }

        [DataMember]
        public string Name { get; init; }

        [DataMember]
        public int Slots { get; init; }

        [DataMember]
        public List<Item> Items { get; init; }
    }
}

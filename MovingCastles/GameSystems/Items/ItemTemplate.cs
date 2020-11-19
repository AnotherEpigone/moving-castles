using System.Runtime.Serialization;

namespace MovingCastles.GameSystems.Items
{
    [DataContract]
    public class ItemTemplate
    {
        public ItemTemplate(
            string id,
            string name,
            string description,
            int glyph)
        {
            Id = id;
            Name = name;
            Description = description;
            Glyph = glyph;
        }

        [DataMember] public string Id { get; }

        [DataMember] public string Name { get; }

        [DataMember] public string Description { get; }

        [DataMember] public int Glyph { get; }
    }
}

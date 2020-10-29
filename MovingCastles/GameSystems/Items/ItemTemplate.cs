namespace MovingCastles.GameSystems.Items
{
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

        public string Id { get; }

        public string Name { get; }

        public string Description { get; }

        public int Glyph { get; }
    }
}

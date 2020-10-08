using Microsoft.Xna.Framework;

namespace MovingCastles.Entities
{
    public class ActorTemplate
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Glyph { get; set; }

        public Color NameColor { get; set; }

        public float MaxHealth { get; set; }

        public float Health { get; set; }
    }
}

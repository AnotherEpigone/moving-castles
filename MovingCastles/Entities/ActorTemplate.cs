using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MovingCastles.Entities
{
    public class ActorTemplate
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Glyph { get; set; }

        public Color NameColor { get; set; }

        public System.Func<List<object>> CreateComponents { get; set; }
    }
}

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MovingCastles.Entities
{
    public class DoodadTemplate
    {
        public DoodadTemplate(
            string id,
            string name,
            int glyph,
            Color nameColor,
            bool walkable,
            bool transparent)
        {
            Id = id;
            Name = name;
            Glyph = glyph;
            NameColor = nameColor;
            Walkable = walkable;
            Transparent = transparent;
        }

        public string Id { get; }

        public string Name { get; }

        public int Glyph { get; }

        public Color NameColor { get; }

        public bool Walkable { get; }

        public bool Transparent { get; }
    }
}

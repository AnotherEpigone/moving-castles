using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MovingCastles.Entities
{
    public class ActorTemplate
    {
        public ActorTemplate(
            string id,
            string name,
            int glyph,
            Color nameColor,
            System.Func<List<object>> createComponents,
            string factionName)
        {
            Id = id;
            Name = name;
            Glyph = glyph;
            NameColor = nameColor;
            CreateComponents = createComponents;
            FactionName = factionName;
        }

        public string Id { get; }

        public string Name { get; }

        public int Glyph { get; }

        public Color NameColor { get; }

        public string FactionName { get; }

        public System.Func<List<object>> CreateComponents { get; }
    }
}

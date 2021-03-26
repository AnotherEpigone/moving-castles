using Microsoft.Xna.Framework;
using System;
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
            Func<List<object>> createComponents,
            string factionName,
            List<SubTileTemplate> subTiles)
        {
            Id = id;
            Name = name;
            Glyph = glyph;
            NameColor = nameColor;
            CreateComponents = createComponents;
            FactionName = factionName;
            SubTiles = subTiles;
        }

        public string Id { get; }

        public string Name { get; }

        public int Glyph { get; }

        public Color NameColor { get; }

        public string FactionName { get; }

        public Func<List<object>> CreateComponents { get; }

        public List<SubTileTemplate> SubTiles { get; }
    }
}

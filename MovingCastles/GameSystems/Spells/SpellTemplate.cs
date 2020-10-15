using MovingCastles.GameSystems.Spells.SpellEffects;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Spells
{
    public class SpellTemplate
    {
        public SpellTemplate(
            string id,
            string name,
            string description,
            int iconGlyph,
            List<ISpellEffect> effects)
        {
            Id = id;
            Name = name;
            Description = description;
            IconGlyph = iconGlyph;
            Effects = effects;
        }

        public string Id { get; }

        public string Name { get; }

        public string Description { get; }

        public int IconGlyph { get; }

        public List<ISpellEffect> Effects { get; }
    }
}

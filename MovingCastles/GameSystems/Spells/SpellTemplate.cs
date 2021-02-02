using MovingCastles.GameSystems.Spells.SpellEffects;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Spells
{
    public record SpellTemplate
    {
        public SpellTemplate(
            string id,
            string name,
            string description,
            int iconGlyph,
            int endowmentCost,
            ITargettingStyle targettingStyle,
            List<ISpellEffect> effects,
            int baseCastTime)
        {
            Id = id;
            Name = name;
            Description = description;
            IconGlyph = iconGlyph;
            EndowmentCost = endowmentCost;
            TargettingStyle = targettingStyle;
            Effects = effects;
            BaseCastTime = baseCastTime;
        }

        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int IconGlyph { get; }
        public int EndowmentCost { get; }
        public ITargettingStyle TargettingStyle { get; }
        public List<ISpellEffect> Effects { get; }
        public int BaseCastTime { get; }
    }
}

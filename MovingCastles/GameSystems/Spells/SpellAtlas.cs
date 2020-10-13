using System.Collections.Generic;

namespace MovingCastles.GameSystems.Spells
{
    public static class SpellAtlas
    {
        public static SpellTemplate ConjureFlame => new SpellTemplate(
            id: "SPELL_CONJURE_FLAME",
            name: "Conjure flame",
            description: "To manifest the power of the wellspring in flame is as natural as breath for the magi.",
            iconGlyph: 0,
            effects: new List<ISpellEffect>
                {
                    new DamageTargetSpellEffect(10),
                });
    }
}

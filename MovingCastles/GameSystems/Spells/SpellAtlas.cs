using MovingCastles.GameSystems.Spells.SpellEffects;
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
            targettingStyle: new TargettingStyle(true, TargetMode.SingleTarget),
            effects: new List<ISpellEffect>
                {
                    new DamageTargetSpellEffect(10),
                });
        public static SpellTemplate RayOfFrost => new SpellTemplate(
            id: "SPELL_RAY_OF_FROST",
            name: "Ray of Frost",
            description: "...",
            iconGlyph: 0,
            targettingStyle: new TargettingStyle(true, TargetMode.Projectile),
            effects: new List<ISpellEffect>
                {
                    new DamageTargetSpellEffect(10),
                });
        public static SpellTemplate EtherealStep => new SpellTemplate(
            id: "SPELL_ETHEREAL_STEP",
            name: "Ethereal step",
            description: "A most practical application of basic realmatic attunement. The magus steps briefly into the ethereal realm, utilizes a split second of mental movement, and re-enters the material plane at a nearby place.",
            iconGlyph: 0,
            targettingStyle: new TargettingStyle(false, TargetMode.SingleTarget),
            effects: new List<ISpellEffect>
                {
                    new TeleportToTargetSpellEffect(),
                });
    }
}

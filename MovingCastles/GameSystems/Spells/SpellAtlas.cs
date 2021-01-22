using MovingCastles.GameSystems.Spells.SpellEffects;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovingCastles.GameSystems.Spells
{
    public static class SpellAtlas
    {
        static SpellAtlas()
        {
            SpellsById = typeof(SpellAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<SpellTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i);
        }

        public static Dictionary<string, SpellTemplate> SpellsById { get; }

        public static SpellTemplate ConjureFlame => new SpellTemplate(
            id: "SPELL_CONJURE_FLAME",
            name: "Conjure flame",
            description: "To manifest the power of the wellspring in flame is as natural as breath for the magi.",
            iconGlyph: 0,
            endowmentCost: 4,
            targettingStyle: new TargettingStyle(true, TargetMode.SingleTarget, 3),
            effects: new List<ISpellEffect>
                {
                    new DamageTargetSpellEffect(10),
                });
        public static SpellTemplate RayOfFrost => new SpellTemplate(
            id: "SPELL_RAY_OF_FROST",
            name: "Ray of Frost",
            description: "TODO",
            iconGlyph: 0,
            endowmentCost: 18,
            targettingStyle: new TargettingStyle(true, TargetMode.Projectile, 8),
            effects: new List<ISpellEffect>
                {
                    new DamageTargetSpellEffect(10),
                });
        public static SpellTemplate EtherealStep => new SpellTemplate(
            id: "SPELL_ETHEREAL_STEP",
            name: "Ethereal step",
            description: "A most practical application of basic realmatic attunement. The magus steps briefly into the ethereal realm, utilizes a split second of mental movement, and re-enters the material plane at a nearby place.",
            iconGlyph: 0,
            endowmentCost: 8,
            targettingStyle: new TargettingStyle(false, TargetMode.SingleTarget, 10),
            effects: new List<ISpellEffect>
                {
                    new TeleportToTargetSpellEffect(),
                });
    }
}

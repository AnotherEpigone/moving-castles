using MovingCastles.GameSystems.Spells.SpellEffects;
using MovingCastles.Text;
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
            description: Gui.Spell_ConjureFlame_Desc,
            iconGlyph: 0,
            endowmentCost: 4,
            targettingStyle: new TargettingStyle(true, TargetMode.SingleTarget, 3),
            effects: new List<ISpellEffect>
                {
                    new DamageTargetSpellEffect(8),
                    new BurningSpellEffect(1, 3),
                },
            baseCastTime: 100);
        public static SpellTemplate RayOfFrost => new SpellTemplate(
            id: "SPELL_FREEZING_WIND",
            name: "Freezing wind",
            description: Gui.Spell_FreezingWind_Desc,
            iconGlyph: 0,
            endowmentCost: 8,
            targettingStyle: new TargettingStyle(true, TargetMode.Projectile, 8),
            effects: new List<ISpellEffect>
                {
                    new DamageTargetSpellEffect(5),
                },
            baseCastTime: 300);
        public static SpellTemplate EtherealStep => new SpellTemplate(
            id: "SPELL_ETHEREAL_STEP",
            name: "Ethereal step",
            description: Gui.Spell_EtherealStep_Desc,
            iconGlyph: 0,
            endowmentCost: 8,
            targettingStyle: new TargettingStyle(false, TargetMode.SingleTarget, 10),
            effects: new List<ISpellEffect>
                {
                    new TeleportToTargetSpellEffect(),
                },
            baseCastTime: 100);
        public static SpellTemplate Fireball => new SpellTemplate(
            id: "SPELL_FIREBALL",
            name: "Fireball",
            description: Gui.Spell_EtherealStep_Desc,
            iconGlyph: 0,
            endowmentCost: 25,
            targettingStyle: new TargettingStyle(false, TargetMode.SingleTarget, 10),
            effects: new List<ISpellEffect>
                {
                },
            baseCastTime: 250);
        public static SpellTemplate Haste => new SpellTemplate(
            id: "SPELL_HASTE",
            name: "Haste",
            description: Gui.Spell_Haste_Desc,
            iconGlyph: 0,
            endowmentCost: 25,
            targettingStyle: new TargettingStyle(false, TargetMode.SingleTarget, 10),
            effects: new List<ISpellEffect>
            {
            },
            baseCastTime: 250);
    }
}

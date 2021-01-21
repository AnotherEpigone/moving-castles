using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components.Stats;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using MovingCastles.Ui;

namespace MovingCastles.GameSystems.Spells.SpellEffects
{
    public class DamageTargetSpellEffect : ISpellEffect
    {
        private readonly float _damage;

        public DamageTargetSpellEffect(float damage)
        {
            _damage = damage;
        }

        public void Apply(McEntity caster, SpellTemplate spell, DungeonMap map, HitResult hitResult, Coord targetCoord, ILogManager logManager)
        {
            var target = map.GetEntity<McEntity>(targetCoord);
            var targetName = target.ColoredName;
            var targetHealth = target?.GetGoRogueComponent<IHealthComponent>();
            if (targetHealth == null)
            {
                return;
            }

            var damage = _damage;
            switch (hitResult)
            {
                case HitResult.Hit:
                    logManager.EventLog($"{caster.ColoredName}'s {spell.Name} {ColorHelper.GetParserString("hit", ColorHelper.ImportantAction)} {targetName} for {damage:F0} damage.");
                    targetHealth.ApplyDamage(damage, logManager);
                    break;
                case HitResult.Glance:
                    damage /= 4;
                    logManager.EventLog($"{caster.ColoredName}'s {spell.Name} hit {targetName} with a {ColorHelper.GetParserString("glancing blow", ColorHelper.ImportantAction)} for {damage:F0} damage.");
                    targetHealth.ApplyDamage(damage, logManager);
                    break;
                case HitResult.Miss:
                    logManager.EventLog($"{caster.ColoredName}'s {spell.Name} {ColorHelper.GetParserString("missed", ColorHelper.ImportantAction)} {targetName}.");
                    break;
                case HitResult.Crit:
                    damage *= 2;
                    logManager.EventLog($"{caster.ColoredName}'s {spell.Name} hit {targetName} with a {ColorHelper.GetParserString("critical blow", ColorHelper.ImportantAction)} for {damage:F0} damage.");
                    targetHealth.ApplyDamage(damage, logManager);
                    break;
            }
        }
    }
}

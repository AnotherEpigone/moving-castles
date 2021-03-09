using GoRogue;
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

        public string Description => $"Deals {_damage:0.#} damage to the target on a hit.";

        public void Apply(
            IDungeonMaster dungeonMaster,
            McEntity caster,
            SpellTemplate spell,
            McMap map,
            HitResult hitResult,
            Coord targetCoord,
            ILogManager logManager)
        {
            var target = map.GetActor(targetCoord);
            if (target == null)
            {
                return;
            }

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
                    logManager.CombatLog($"{caster.ColoredName}'s {spell.Name} {ColorHelper.GetParserString("hit", ColorHelper.ImportantAction)} {targetName} for {damage:F0} damage.");
                    targetHealth.ApplyDamage(damage, logManager);
                    break;
                case HitResult.Glance:
                    damage /= 4;
                    logManager.CombatLog($"{caster.ColoredName}'s {spell.Name} hit {targetName} with a {ColorHelper.GetParserString("glancing blow", ColorHelper.ImportantAction)} for {damage:F0} damage.");
                    targetHealth.ApplyDamage(damage, logManager);
                    break;
                case HitResult.Miss:
                    logManager.CombatLog($"{caster.ColoredName}'s {spell.Name} {ColorHelper.GetParserString("missed", ColorHelper.ImportantAction)} {targetName}.");
                    break;
                case HitResult.Crit:
                    damage *= 2;
                    logManager.CombatLog($"{caster.ColoredName}'s {spell.Name} hit {targetName} with a {ColorHelper.GetParserString("critical blow", ColorHelper.ImportantAction)} for {damage:F0} damage.");
                    targetHealth.ApplyDamage(damage, logManager);
                    break;
            }
        }
    }
}

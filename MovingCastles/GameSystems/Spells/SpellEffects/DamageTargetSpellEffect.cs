using GoRogue;
using MovingCastles.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;

namespace MovingCastles.GameSystems.Spells.SpellEffects
{
    public class DamageTargetSpellEffect : ISpellEffect
    {
        private readonly float _damage;

        public DamageTargetSpellEffect(float damage)
        {
            _damage = damage;
        }

        public void Apply(McEntity caster, SpellTemplate spell, DungeonMap map, Coord targetCoord, ILogManager logManager)
        {
            var target = map.GetEntity<McEntity>(targetCoord);
            var targetHealth = target?.GetGoRogueComponent<IHealthComponent>();
            if (targetHealth == null)
            {
                return;
            }

            logManager.EventLog($"{caster.ColoredName}'s {spell.Name} hit {target.ColoredName} for {_damage:F0} damage.");
            targetHealth.ApplyDamage(_damage, logManager);
        }
    }
}

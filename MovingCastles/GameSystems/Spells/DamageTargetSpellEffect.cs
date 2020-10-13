using MovingCastles.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.GameSystems.Spells
{
    public class DamageTargetSpellEffect : ISpellEffect
    {
        private readonly float _damage;

        public DamageTargetSpellEffect(float damage)
        {
            _damage = damage;
        }

        public void Apply(McEntity caster, McEntity target, ILogManager logManager)
        {
            var targetHealth = target.GetGoRogueComponent<IHealthComponent>();
            if (targetHealth == null)
            {
                return;
            }

            targetHealth.ApplyDamage(_damage, logManager);
        }
    }
}

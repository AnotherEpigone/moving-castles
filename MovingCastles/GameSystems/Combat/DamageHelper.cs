using MovingCastles.Components.Stats;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.GameSystems.Combat
{
    public static class DamageHelper
    {
        public static void DoDamage(McEntity target, float damage, ILogManager logManager)
        {
            var healthComponent = target.GetGoRogueComponent<IHealthComponent>();
            healthComponent.ApplyDamage(damage, logManager);
        }
    }
}

using MovingCastles.Components;
using MovingCastles.Entities;

namespace MovingCastles.GameSystems.TurnBasedGame
{
    public class Combat : ICombat
    {
        public void Rest(McEntity entity)
        {
            var health = entity.GetGoRogueComponent<IHealthComponent>();
            if (health == null)
            {
                return;
            }

            health.ApplyHealing(health.BaseRegen);
        }
    }
}

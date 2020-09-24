using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace MovingCastles.Components
{
    public class MeleeAttackerComponent : IGameObjectComponent, IMeleeAttackerComponent
    {
        private readonly int _damage;

        public MeleeAttackerComponent(int damage)
        {
            _damage = damage;
        }

        public IGameObject Parent { get; set; }

        public float GetDamage()
        {
            return _damage;
        }
    }
}

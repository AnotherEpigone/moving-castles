using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;

namespace MovingCastles.Components
{
    public class MeleeAttackerComponent : IGameObjectComponent, IMeleeAttackerComponent
    {
        public IGameObject Parent { get; set; }

        public float GetDamage()
        {
            return 1f;
        }
    }
}

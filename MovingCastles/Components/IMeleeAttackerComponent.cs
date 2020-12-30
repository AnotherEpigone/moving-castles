using GoRogue.GameFramework.Components;

namespace MovingCastles.Components
{
    public interface IMeleeAttackerComponent : IGameObjectComponent
    {
        float GetDamage();
    }
}
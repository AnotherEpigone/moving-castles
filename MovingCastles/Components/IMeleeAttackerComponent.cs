using GoRogue.GameFramework.Components;
using Troschuetz.Random;

namespace MovingCastles.Components
{
    public interface IMeleeAttackerComponent : IGameObjectComponent
    {
        float GetDamage(IGenerator rng);
    }
}
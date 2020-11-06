using GoRogue.GameFramework.Components;
using MovingCastles.Entities;

namespace MovingCastles.Components.Triggers
{
    public interface IBumpTriggeredComponent : IGameObjectComponent
    {
        void Bump(McEntity bumpingEntity);
    }
}

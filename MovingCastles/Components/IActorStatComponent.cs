using GoRogue.GameFramework.Components;

namespace MovingCastles.Components
{
    public interface IActorStatComponent : IGameObjectComponent
    {
        int WalkSpeed { get; }
    }
}
using GoRogue.GameFramework.Components;

namespace MovingCastles.Components.Stats
{
    public interface IActorStatComponent : IGameObjectComponent
    {
        float WalkSpeed { get; }
    }
}
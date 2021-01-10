using GoRogue.GameFramework.Components;

namespace MovingCastles.Components.Stats
{
    public interface IActorStatComponent : IGameObjectComponent
    {
        int WalkSpeed { get; }
    }
}
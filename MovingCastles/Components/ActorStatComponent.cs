namespace MovingCastles.Components
{
    public class ActorStatComponent : IActorStatComponent
    {
        public ActorStatComponent(
            int walkSpeed)
        {
            WalkSpeed = walkSpeed;
        }

        public int WalkSpeed { get; }
    }
}

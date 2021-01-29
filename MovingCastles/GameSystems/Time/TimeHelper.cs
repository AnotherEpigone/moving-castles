using MovingCastles.Components.Stats;
using MovingCastles.Entities;

namespace MovingCastles.GameSystems.Time
{
    public static class TimeHelper
    {
        public const int Walk = 100;
        public const int Wait = 100;
        public const int Attack = 200;
        public const int Interact = 50;

        public static int GetWalkTime(McEntity entity)
        {
            var speed = entity.GetGoRogueComponent<IActorStatComponent>()?.WalkSpeed ?? 1;
            return (int)(Walk / speed);
        }
    }
}

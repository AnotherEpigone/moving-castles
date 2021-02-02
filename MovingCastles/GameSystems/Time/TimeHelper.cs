using MovingCastles.Components.Stats;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;

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

        public static int GetAttackTime(McEntity entity)
        {
            var speed = entity.GetGoRogueComponent<IActorStatComponent>()?.AttackSpeed ?? 1;
            return (int)(Attack / speed);
        }

        public static int GetSpellcastingTime(McEntity entity, SpellTemplate spell)
        {
            var speed = entity.GetGoRogueComponent<IActorStatComponent>()?.CastSpeed ?? 1;
            return (int)(spell.BaseCastTime / speed);
        }
    }
}

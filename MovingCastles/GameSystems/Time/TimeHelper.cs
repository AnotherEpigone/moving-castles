using MovingCastles.Components.Effects;
using MovingCastles.Components.Stats;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using System.Linq;

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
            var modifiers = entity.GetGoRogueComponents<ISpeedModifier>();

            var negativeModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m < 0)
                .Sum(m => m);

            var positiveModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m > 0)
                .Sum(m => m);

            var effectiveSpeed = speed * positiveModifier * negativeModifier;

            return (int)(Walk / effectiveSpeed);
        }

        public static int GetAttackTime(McEntity entity)
        {
            var speed = entity.GetGoRogueComponent<IActorStatComponent>()?.AttackSpeed ?? 1;
            var modifiers = entity.GetGoRogueComponents<ISpeedModifier>();

            var negativeModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m < 0)
                .Sum(m => m);

            var positiveModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m > 0)
                .Sum(m => m);

            var effectiveSpeed = speed * positiveModifier * negativeModifier;

            return (int)(Attack / effectiveSpeed);
        }

        public static int GetCastTime(McEntity entity, SpellTemplate spell)
        {
            var speed = entity.GetGoRogueComponent<IActorStatComponent>()?.CastSpeed ?? 1;
            var modifiers = entity.GetGoRogueComponents<ISpeedModifier>();

            var negativeModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m < 0)
                .Sum(m => m);

            var positiveModifier = 1 + modifiers
                .Select(m => m.Modifier)
                .Where(m => m > 0)
                .Sum(m => m);

            var effectiveSpeed = speed * positiveModifier * negativeModifier;

            return (int)(spell.BaseCastTime / effectiveSpeed);
        }
    }
}

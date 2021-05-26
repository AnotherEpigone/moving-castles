using MovingCastles.Components.Effects;
using MovingCastles.Entities;
using System;
using Troschuetz.Random;

namespace MovingCastles.GameSystems.Combat
{
    public enum HitResult
    {
        Crit,
        Hit,
        Glance,
        Miss,
    }

    public class HitMan : IHitMan
    {
        private const int BaseCritChance = 1;
        private const int BaseHitChance = 100;
        private const int BaseGlanceChance = 10;
        private const int BaseMissChance = 100;

        public HitResult Get(McEntity attacker, McEntity defender, IGenerator rng)
        {
            var glanceChance = BaseGlanceChance;
            foreach (var deflectEffect in defender.GetGoRogueComponents<IDeflectEffect>())
            {
                glanceChance = Math.Max(0, glanceChance + deflectEffect.DeflectModifier);
            }

            var totalChance = BaseHitChance + BaseMissChance;
            var result = rng.Next(totalChance);
            if (result < BaseCritChance)
            {
                return HitResult.Crit;
            }

            var hitThreshold = BaseHitChance - glanceChance;
            if (result < hitThreshold)
            {
                return HitResult.Hit;
            }

            var glanceThreshold = BaseHitChance;
            if (result < glanceThreshold)
            {
                return HitResult.Glance;
            }

            return HitResult.Miss;
        }
    }
}

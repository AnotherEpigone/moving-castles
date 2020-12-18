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

    public static class HitMan
    {
        private const uint BaseCritChance = 10;
        private const uint BaseHitChance = 100;
        private const uint BaseGlanceChance = 10;
        private const uint BaseMissChance = 90;

        public static HitResult Get(IGenerator rng)
        {
            var totalChance = BaseCritChance + BaseHitChance + BaseGlanceChance + BaseMissChance;
            var result = rng.NextUInt(totalChance);
            if (result < BaseCritChance)
            {
                return HitResult.Crit;
            }

            var hitThreshold = BaseCritChance + BaseHitChance;
            if (result < hitThreshold)
            {
                return HitResult.Hit;
            }

            var glanceThreshold = hitThreshold + BaseGlanceChance;
            if (result < glanceThreshold)
            {
                return HitResult.Glance;
            }

            return HitResult.Miss;
        }
    }
}

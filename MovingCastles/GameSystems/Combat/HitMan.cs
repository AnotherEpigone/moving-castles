using Troschuetz.Random;

namespace MovingCastles.GameSystems.Combat
{
    public enum HitResult
    {
        Hit,
        Glance,
        Miss,
    }

    public static class HitMan
    {
        private const uint BaseHitChance = 100;
        private const uint BaseGlanceChance = 10;
        private const uint BaseMissChance = 90;

        public static HitResult Get(IGenerator rng)
        {
            var totalChance = BaseHitChance + BaseGlanceChance + BaseMissChance;
            var result = rng.NextUInt(totalChance);
            if (result < BaseHitChance)
            {
                return HitResult.Hit;
            }

            var glanceThreshold = BaseHitChance + BaseGlanceChance;
            if (result < glanceThreshold)
            {
                return HitResult.Glance;
            }

            return HitResult.Miss;
        }
    }
}

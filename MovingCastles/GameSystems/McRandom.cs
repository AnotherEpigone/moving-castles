using System;

namespace MovingCastles.GameSystems
{
    public static class McRandom
    {
        public static int GetSeed()
        {
            return (int)(DateTime.UtcNow - new DateTime(2014, 5, 31)).TotalSeconds;
        }
    }
}

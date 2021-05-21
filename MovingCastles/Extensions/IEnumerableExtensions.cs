using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random;

namespace MovingCastles.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> target, IGenerator rng)
        {
            return target.OrderBy(_ => rng.Next());
        }
    }
}

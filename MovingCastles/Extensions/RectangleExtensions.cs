using GoRogue;
using System.Collections.Generic;

namespace MovingCastles.Extensions
{
    public static class RectangleExtensions
    {
        public static IEnumerable<Coord> InteriorPositions(this Rectangle rect)
        {
            if (rect.Width < 3 || rect.Height < 3)
            {
                yield break;
            }

            for (int x = rect.X + 1; x < rect.X + rect.Width - 1; ++x)
            {
                for (int y = rect.Y + 1; y < rect.Y + rect.Height - 1; ++y)
                {
                    yield return new Coord(x, y);
                }
            }
        }
    }
}

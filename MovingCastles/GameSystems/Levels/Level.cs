using GoRogue;
using MovingCastles.Maps;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Levels
{
    public record Level(
            string Id,
            string Name,
            int Seed,
            IList<Rectangle> Rooms,
            IList<Coord> Doors,
            DungeonMap Map);
}

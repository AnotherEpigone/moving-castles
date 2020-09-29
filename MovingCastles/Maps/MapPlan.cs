using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Maps
{
    public class MapPlan : IMapPlan
    {
        public MapPlan()
        {
            FloorItems = new List<ItemTemplate>();
        }

        public List<ItemTemplate> FloorItems { get; }
    }
}

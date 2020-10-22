using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Maps
{
    public class MapPlanFactory : IMapPlanFactory
    {
        public MapPlan Create(
            MapTemplate template,
            IDictionary<string, ItemTemplate> items)
        {
            var map = new MapPlan();
            template.FloorItems.ForEach(i => map.FloorItems.Add(items[i]));

            return map;
        }
    }
}

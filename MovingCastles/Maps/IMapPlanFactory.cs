using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Maps
{
    public interface IMapPlanFactory
    {
        MapPlan Create(MapTemplate template, IDictionary<string, ItemTemplate> items);
    }
}
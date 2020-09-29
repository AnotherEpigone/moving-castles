using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Maps
{
    public interface IMapPlan
    {
        /// <summary>
        /// Items that can appear scattered on the floor throughout the map
        /// </summary>
        List<ItemTemplate> FloorItems { get; }
    }
}
using System.Collections.Generic;

namespace MovingCastles.Maps
{
    public class MapTemplate
    {
        public MapTemplate(
            string id,
            List<string> floorItems)
        {
            Id = id;
            FloorItems = floorItems;
        }

        public string Id { get; }

        public List<string> FloorItems { get; }
    }
}

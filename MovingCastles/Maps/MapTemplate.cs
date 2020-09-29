using System.Collections.Generic;
using System.Xml.Serialization;

namespace MovingCastles.Maps
{
    public class MapTemplate
    {
        public string Id { get; set; }

        [XmlArray("FloorItems")]
        [XmlArrayItem("Item")]
        public List<string> FloorItems { get; set; }
    }
}

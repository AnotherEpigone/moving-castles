using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MovingCastles.Maps
{
    public class MapTemplateLoader
    {
        private const string MapTemplateXml = "Content\\MapTemplates.xml";

        public Dictionary<string, MapTemplate> Load()
        {
            var serializer = new XmlSerializer(typeof(MapTemplates));
            using var file = System.IO.File.OpenRead(MapTemplateXml);
            return ((List<MapTemplate>)serializer.Deserialize(file))
                .ToDictionary(
                    map => map.Id,
                    map => map);
        }
    }

    [XmlRoot("MapTemplates")]
    public class MapTemplates : List<MapTemplate> { }
}

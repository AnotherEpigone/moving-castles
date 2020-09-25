using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MovingCastles.GameSystems.Items
{
    public class ItemTemplateLoader
    {
        private const string ItemTemplateXml = "Content\\ItemTemplates.xml";

        public Dictionary<string, ItemTemplate> Load()
        {
            var serializer = new XmlSerializer(typeof(ItemTemplates));
            using var file = System.IO.File.OpenRead(ItemTemplateXml);
            return ((List<ItemTemplate>)serializer.Deserialize(file))
                .ToDictionary(
                    item => item.Id,
                    item => item);
        }
    }

    [XmlRoot("ItemTemplates")]
    public class ItemTemplates : List<ItemTemplate> { }
}

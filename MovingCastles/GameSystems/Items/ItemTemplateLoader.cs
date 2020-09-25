using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MovingCastles.GameSystems.Items
{
    public class ItemTemplateLoader
    {
        private const string ItemTemplateXml = "Content\\ItemTemplates.xml";

        public List<ItemTemplate> Load()
        {
            var serializer = new XmlSerializer(typeof(ItemTemplates));
            using var file = System.IO.File.OpenRead(ItemTemplateXml);
            return (List<ItemTemplate>)serializer.Deserialize(file);
        }
    }

    [XmlRoot("ItemTemplates")]
    public class ItemTemplates : List<ItemTemplate> { }
}

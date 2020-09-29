using System.Collections.Generic;

namespace MovingCastles.GameSystems.Items
{
    public interface IItemTemplateLoader
    {
        Dictionary<string, ItemTemplate> Load();
    }
}
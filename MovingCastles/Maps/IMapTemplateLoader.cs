using System.Collections.Generic;

namespace MovingCastles.Maps
{
    public interface IMapTemplateLoader
    {
        Dictionary<string, MapTemplate> Load();
    }
}
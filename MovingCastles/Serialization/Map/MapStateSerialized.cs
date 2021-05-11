

using MovingCastles.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Serialization.Map
{
    public class MapStateJsonConverter : JsonConverter<MapState>
    {
        public override void WriteJson(JsonWriter writer, MapState value, JsonSerializer serializer) => serializer.Serialize(writer, (MapStateSerialized)value);

        public override MapState ReadJson(JsonReader reader, System.Type objectType, MapState existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<MapStateSerialized>(reader);
    }

    [DataContract]
    public class MapStateSerialized
    {
        [DataMember] public string Id;
        [DataMember] public int Seed;
        [DataMember] public int Height;
        [DataMember] public int Width;
        [DataMember] public bool[] Explored;
        [DataMember] public Dictionary<Guid, McEntity> Entities;
        [DataMember] public List<McEntity> SubTiles;
        [DataMember] public List<Door> Doors;
        [DataMember] public string StructureId;

        public static implicit operator MapStateSerialized(MapState mapState)
        {
            var serialized = new MapStateSerialized
            {
                Id = mapState.Id,
                Seed = mapState.Seed,
                Height = mapState.Height,
                Width = mapState.Width,
                Explored = mapState.Explored,
                Entities = mapState.Entities
                    .Where(e => !e.IsSubTile)
                    .ToDictionary(
                        e => e.UniqueId,
                        e => e),
                SubTiles = mapState.Entities
                    .Where(e => e.IsSubTile)
                    .ToList(),
                Doors = mapState.Doors,
                StructureId = mapState.StructureId,
            };

            return serialized;
        }

        public static implicit operator MapState(MapStateSerialized serialized)
        {
            var mapState = new MapState
            {
                Id = serialized.Id,
                Seed = serialized.Seed,
                Height = serialized.Height,
                Width = serialized.Width,
                Explored = serialized.Explored,
                Entities = serialized.Entities.Values.Concat(serialized.SubTiles).ToList(),
                Doors = serialized.Doors,
                StructureId = serialized.StructureId,
            };

            foreach (var subTile in serialized.SubTiles)
            {
                var anchor = serialized.Entities[subTile.UniqueId];
                anchor.SubTiles.Add(subTile);
                subTile.Anchor = anchor;
            }

            return mapState;
        }
    }
}

using MovingCastles.GameSystems.Time;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MovingCastles.Serialization.Map
{
    public class TimeMasterJsonConverter : JsonConverter<TimeMaster>
    {
        public override void WriteJson(JsonWriter writer, TimeMaster value, JsonSerializer serializer) => serializer.Serialize(writer, (TimeMasterSerialized)value);

        public override TimeMaster ReadJson(JsonReader reader, System.Type objectType, TimeMaster existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<TimeMasterSerialized>(reader);
    }

    [DataContract]
    public class TimeMasterSerialized
    {
        [DataMember] public long Ticks;
        [DataMember] public WizardTurnTimeMasterNode WizardNode;
        [DataMember] public List<EntityTurnTimeMasterNode> EntityNodes;

        public static implicit operator TimeMasterSerialized(TimeMaster timeMaster)
        {
            var serialized = new TimeMasterSerialized()
            {
                Ticks = timeMaster.JourneyTime.Ticks,
                EntityNodes = new List<EntityTurnTimeMasterNode>(),
            };

            foreach (var node in timeMaster.Nodes)
            {
                switch (node)
                {
                    case WizardTurnTimeMasterNode w:
                        serialized.WizardNode = w;
                        break;
                    case EntityTurnTimeMasterNode e:
                        serialized.EntityNodes.Add(e);
                        break;
                    default:
                        throw new System.NotSupportedException($"Unsupported time master node type: {node.GetType()}");
                }
            }

            return serialized;
        }

        public static implicit operator TimeMaster(TimeMasterSerialized serialized)
        {
            var timeMaster = new TimeMaster(serialized.Ticks);
            if (serialized.WizardNode != null)
            {
                timeMaster.Enqueue(serialized.WizardNode);
            }

            foreach (var node in serialized.EntityNodes)
            {
                timeMaster.Enqueue(node);
            }

            return timeMaster;
        }
    }
}

using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Serialization;
using Newtonsoft.Json;

namespace MovingCastles.Components.Effects
{
    public class HealthRegenEffect : IHealthRegenEffect
    {
        public HealthRegenEffect(float value)
        {
            Value = value;
        }

        public HealthRegenEffect(SerializedObject state)
            : this(JsonConvert.DeserializeObject<float>(state.Value)) { }

        public float Value { get; }

        public IGameObject Parent { get; set; }

        public string GetDescription() => $"Health regen {Value}";

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(HealthRegenEffect),
            State = JsonConvert.SerializeObject(Value),
        };
    }
}

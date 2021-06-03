using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Serialization;
using Newtonsoft.Json;

namespace MovingCastles.Components.Effects
{
    public class EndowmentRegenEffect : IEndowmentRegenEffect
    {
        public EndowmentRegenEffect(float value)
        {
            Value = value;
        }

        public EndowmentRegenEffect(SerializedObject state)
            : this(JsonConvert.DeserializeObject<float>(state.Value)) { }

        public float Value { get; }

        public IGameObject Parent { get; set; }

        public string GetDescription() => $"Endowment regen {Value}";

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(EndowmentRegenEffect),
            State = JsonConvert.SerializeObject(Value),
        };
    }
}

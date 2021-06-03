using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Serialization;
using Newtonsoft.Json;

namespace MovingCastles.Components.Effects
{
    public class DeflectEffect : IDeflectEffect
    {
        public DeflectEffect(SerializedObject state)
            : this(JsonConvert.DeserializeObject<int>(state.Value)) { }

        public DeflectEffect(int modifier)
        {
            DeflectModifier = modifier;
        }

        public int DeflectModifier { get; }

        public IGameObject Parent { get; set; }

        public string GetDescription() => $"Deflect {DeflectModifier}";

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(DeflectEffect),
            State = JsonConvert.SerializeObject(DeflectModifier),
        };
    }
}

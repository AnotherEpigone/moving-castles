using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Serialization;
using Newtonsoft.Json;

namespace MovingCastles.Components.Effects
{
    public class FovRangeEffect : IFovRangeEffect
    {
        public FovRangeEffect(int modifier)
        {
            Modifier = modifier;
        }

        public FovRangeEffect(SerializedObject state)
            : this(JsonConvert.DeserializeObject<int>(state.Value)) { }

        public int Modifier { get; }

        public IGameObject Parent { get; set; }

        public string GetDescription() => $"Vision range {Modifier}";

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(FovRangeEffect),
            State = JsonConvert.SerializeObject(Modifier),
        };
    }
}

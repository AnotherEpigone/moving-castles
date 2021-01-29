using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MovingCastles.Components.Stats
{
    public class ActorStatComponent : IActorStatComponent, ISerializableComponent
    {
        public ActorStatComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            WalkSpeed = stateObj.WalkSpeed;
        }

        public ActorStatComponent(
            float walkSpeed)
        {
            WalkSpeed = walkSpeed;
        }

        public float WalkSpeed { get; }

        public IGameObject Parent { get; set; }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(ActorStatComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                WalkSpeed = WalkSpeed,
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public float WalkSpeed;
        }
    }
}

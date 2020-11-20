using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MovingCastles.Components
{
    public class ActorStatComponent : IActorStatComponent, ISerializableComponent
    {
        public ActorStatComponent(string state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state);
            WalkSpeed = stateObj.WalkSpeed;
        }

        public ActorStatComponent(
            int walkSpeed)
        {
            WalkSpeed = walkSpeed;
        }

        public int WalkSpeed { get; }

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
            [DataMember] public int WalkSpeed;
        }
    }
}

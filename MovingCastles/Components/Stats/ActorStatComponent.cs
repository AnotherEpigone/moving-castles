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
            AttackSpeed = stateObj.AttackSpeed;
            CastSpeed = stateObj.CastSpeed;
        }

        public ActorStatComponent(
            float walkSpeed,
            float attackSpeed,
            float castSpeed)
        {
            WalkSpeed = walkSpeed;
            AttackSpeed = attackSpeed;
            CastSpeed = castSpeed;
        }

        public float WalkSpeed { get; }
        public float AttackSpeed { get; }
        public float CastSpeed { get; }

        public IGameObject Parent { get; set; }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(ActorStatComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                WalkSpeed = WalkSpeed,
                AttackSpeed = AttackSpeed,
                CastSpeed = CastSpeed,
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public float WalkSpeed;
            [DataMember] public float AttackSpeed;
            [DataMember] public float CastSpeed;
        }
    }
}

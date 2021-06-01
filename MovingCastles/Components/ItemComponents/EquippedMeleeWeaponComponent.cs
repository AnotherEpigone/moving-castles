using GoRogue.DiceNotation;
using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace MovingCastles.Components.ItemComponents
{
    public class EquippedMeleeWeaponComponent : IEquippedMeleeWeaponComponent
    {
        private readonly string _damage;

        public EquippedMeleeWeaponComponent(string damage, int speedMod)
        {
            _damage = damage;
            Damage = Dice.Parse(_damage);
            SpeedMod = speedMod;
        }

        public EquippedMeleeWeaponComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _damage = stateObj.Damage;
            Damage = Dice.Parse(_damage);
            SpeedMod = stateObj.SpeedMod;
        }

        public IDiceExpression Damage { get; }

        public int SpeedMod { get; }

        public IGameObject Parent { get; set; }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(EquippedMeleeWeaponComponent),
                State = JsonConvert.SerializeObject(new State
                {
                    Damage = _damage,
                    SpeedMod = SpeedMod,
                }),
            };
        }

        [DataContract]
        private class State
        {
            [DataMember] public string Damage;
            [DataMember] public int SpeedMod;
        }
    }
}

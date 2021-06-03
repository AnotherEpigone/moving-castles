using GoRogue.GameFramework;
using MovingCastles.Components.Effects;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace MovingCastles.Components.Stats
{
    public class EndowmentPoolComponent : IEndowmentPoolComponent, ISerializableComponent
    {
        private float _value;

        public EndowmentPoolComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            MaxValue = stateObj.MaxValue;
            Value = stateObj.Value;
            BaseRegen = stateObj.BaseRegen;
        }

        public EndowmentPoolComponent(float maxValue, float value, float baseRegen)
        {
            Value = value;
            MaxValue = maxValue;
            BaseRegen = baseRegen;
        }

        public event EventHandler<float> ValueChanged;

        public IGameObject Parent { get; set; }

        public float Value
        {
            get { return _value; }
            private set
            {
                if (value == _value)
                {
                    return;
                }

                var prevValue = _value;
                _value = value;
                ValueChanged?.Invoke(this, prevValue);
            }
        }

        public float MaxValue { get; }

        public float BaseRegen { get; }

        public void ApplyRegen()
        {
            var regen = BaseRegen;
            if (Parent is McEntity mcParent)
            {
                var regenEffects = mcParent.GetGoRogueComponents<IEndowmentRegenEffect>();
                foreach (var effect in regenEffects)
                {
                    regen += effect.Value;
                }

            }

            regen = Math.Max(0, regen);

            ApplyRestore(regen);
        }

        public void ApplyDrain(float damage)
        {
            Value = Math.Max(0, Value - damage);
        }

        public void ApplyRestore(float healing)
        {
            Value = Math.Min(MaxValue, Value + healing);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(EndowmentPoolComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                MaxValue = MaxValue,
                Value = Value,
                BaseRegen = BaseRegen,
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public float MaxValue;
            [DataMember] public float Value;
            [DataMember] public float BaseRegen;
        }
    }
}

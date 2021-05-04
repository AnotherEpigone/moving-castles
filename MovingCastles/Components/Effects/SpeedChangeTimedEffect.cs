using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Time;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MovingCastles.Components.Effects
{
    public class SpeedChangeTimedEffect : ITimedEffect, ISpeedModifier
    {
        private readonly McTimeSpan _startTime;
        private readonly int _lifetimeTicks;

        public SpeedChangeTimedEffect(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _startTime = new McTimeSpan(stateObj.StartTimeTicks);
            _lifetimeTicks = stateObj.LifetimeTicks;
            Modifier = stateObj.Modifier;
        }

        public SpeedChangeTimedEffect(McTimeSpan startTime, float modifier, int lifetimeTicks)
        {
            _startTime = startTime;
            _lifetimeTicks = lifetimeTicks;
            Modifier = modifier;
        }

        public IGameObject Parent { get; set; }

        public float Modifier { get; }

        public void OnTick(McTimeSpan time, ILogManager logManager, IDungeonMaster dungeonMaster)
        {
            if (time >= (_startTime + _lifetimeTicks))
            {
                Parent.RemoveComponent(this);
            }
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(SpeedChangeTimedEffect),
            State = JsonConvert.SerializeObject(new State()
            {
                StartTimeTicks = _startTime.Ticks,
                LifetimeTicks = _lifetimeTicks,
                Modifier = Modifier,
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public long StartTimeTicks;
            [DataMember] public int LifetimeTicks;
            [DataMember] public float Modifier;
        }
    }
}

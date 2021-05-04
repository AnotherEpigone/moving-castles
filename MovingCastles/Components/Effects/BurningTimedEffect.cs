using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Time;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MovingCastles.Components.Effects
{
    public class BurningTimedEffect : ITimedEffect
    {
        private readonly McTimeSpan _startTime;
        private readonly int _lifetimeTicks;
        private readonly float _dps;
        public BurningTimedEffect(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _startTime = new McTimeSpan(stateObj.StartTimeTicks);
            _lifetimeTicks = stateObj.LifetimeTicks;
            _dps = stateObj.Dps;
        }

        public BurningTimedEffect(McTimeSpan startTime, float dps, int lifetimeTicks)
        {
            _startTime = startTime;
            _lifetimeTicks = lifetimeTicks;
            _dps = dps;
        }

        public IGameObject Parent { get; set; }

        public void OnTick(McTimeSpan time, ILogManager logManager, IDungeonMaster dungeonMaster)
        {
            var mcParent = (McEntity)Parent;
            logManager.CombatLog($"{mcParent.ColoredName} burned for {_dps} damage.");
            DamageHelper.DoDamage(mcParent, _dps, logManager);

            if (time >= (_startTime + _lifetimeTicks))
            {
                Parent.RemoveComponent(this);
            }
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(BurningTimedEffect),
            State = JsonConvert.SerializeObject(new State()
            {
                StartTimeTicks = _startTime.Ticks,
                LifetimeTicks = _lifetimeTicks,
                Dps = _dps,
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public long StartTimeTicks;
            [DataMember] public int LifetimeTicks;
            [DataMember] public float Dps;
        }
    }
}

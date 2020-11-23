using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MovingCastles.Components
{
    public class RangedAttackerComponent : IGameObjectComponent, IRangedAttackerComponent, ISerializableComponent
    {
        private readonly int _damage;
        private readonly int _range;

        public RangedAttackerComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _damage = stateObj.Damage;
            _range = stateObj.Range;
        }

        public RangedAttackerComponent(int damage, int range)
        {
            _damage = damage;
            _range = range;
        }

        public IGameObject Parent { get; set; }

        public bool TryAttack(DungeonMap map, ILogManager logManager)
        {
            if (Parent is not McEntity mcParent
                || map.DistanceMeasurement.Calculate(Parent.Position, map.Player.Position) > _range)
            {
                return false;
            }

            var fov = new FOV(map.TransparencyView);

            // TODO replace 5 with vision radius
            fov.Calculate(Parent.Position, 5);
            if (!fov.BooleanFOV[map.Player.Position])
            {
                return false;
            }

            var targetHealth = map.Player.GetGoRogueComponent<IHealthComponent>();
            targetHealth.ApplyDamage(_damage, logManager);

            logManager.EventLog($"{mcParent.ColoredName} hit {map.Player.ColoredName} for {_damage:F0} damage.");
            return true;
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(RangedAttackerComponent),
            State = JsonConvert.SerializeObject(new State() { Damage = _damage, Range = _range, }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public int Damage;
            [DataMember] public int Range;
        }
    }
}

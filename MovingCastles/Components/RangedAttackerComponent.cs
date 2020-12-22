using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using Microsoft.Xna.Framework;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using MovingCastles.Serialization;
using MovingCastles.Ui;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Troschuetz.Random;

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

        public bool TryAttack(DungeonMap map, IGenerator rng, ILogManager logManager)
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
            var hitResult = HitMan.Get(rng);
            var targetName = (targetHealth.Parent as McEntity)?.ColoredName ?? "something";
            var damage = _damage;
            switch (hitResult)
            {
                case HitResult.Hit:
                    logManager.EventLog($"{mcParent.ColoredName} {ColorHelper.GetParserString("hit", Color.Yellow)} {targetName} for {damage:F0} damage.");
                    targetHealth.ApplyDamage(damage, logManager);
                    break;
                case HitResult.Glance:
                    damage /= 4;
                    logManager.EventLog($"{mcParent.ColoredName} hit {targetName} with a {ColorHelper.GetParserString("glancing blow", Color.Yellow)} for {damage:F0} damage.");
                    targetHealth.ApplyDamage(damage, logManager);
                    break;
                case HitResult.Miss:
                    logManager.EventLog($"{mcParent.ColoredName} {ColorHelper.GetParserString("missed", Color.Yellow)} {targetName}.");
                    break;
                case HitResult.Crit:
                    damage *= 2;
                    logManager.EventLog($"{mcParent.ColoredName} hit {targetName} with a {ColorHelper.GetParserString("critical blow", Color.Yellow)} for {damage:F0} damage.");
                    targetHealth.ApplyDamage(damage, logManager);
                    break;
            }
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

using GoRogue;
using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Stats;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Time;
using MovingCastles.Maps;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Serialization;
using Troschuetz.Random;

namespace MovingCastles.Components.AiComponents
{
    public class WalkAtPlayerAiComponent : IAiComponent, ISerializableComponent
    {
        private readonly int _range;

        public WalkAtPlayerAiComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _range = stateObj.Range;
        }

        public WalkAtPlayerAiComponent(int range)
        {
            _range = range;
        }

        public IGameObject Parent { get; set; }

        public (bool success, int ticks) Run(McMap map, IGenerator rng, ILogManager logManager)
        {
            if (Parent is not McEntity mcParent)
            {
                return (false, -1);
            }

            var outcome = TryGetDirectionAndMove(map, mcParent);
            return outcome switch
            {
                MoveOutcome.Move => (true, TimeHelper.GetWalkTime(mcParent)),
                MoveOutcome.NoMove => (true, TimeHelper.Wait),
                MoveOutcome.Melee => (true, TimeHelper.GetAttackTime(mcParent)),
                _ => throw new System.NotSupportedException($"Unsupported move outcome {outcome}."),
            };
        }

        public MoveOutcome TryGetDirectionAndMove(McMap map, McEntity mcParent)
        {
            var path = map.AStar.ShortestPath(Parent.Position, map.Player.Position);

            Direction direction;
            if (path == null || path.Length > _range)
            {
                return MoveOutcome.NoMove;
            }
            else
            {
                direction = Direction.GetDirection(path.Steps.First() - Parent.Position);
            }

            return mcParent.Move(direction);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(WalkAtPlayerAiComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                Range = _range,
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public int Range;
        }
    }
}

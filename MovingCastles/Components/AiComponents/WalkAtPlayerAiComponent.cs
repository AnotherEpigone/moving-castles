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

        public (bool success, int ticks) Run(DungeonMap map, IGenerator rng, ILogManager logManager)
        {
            if (Parent is not McEntity mcParent)
            {
                return (false, -1);
            }

            var walkSpeed = mcParent.GetGoRogueComponent<IActorStatComponent>()?.WalkSpeed ?? 1;
            var bumped = false;
            System.EventHandler<ItemMovedEventArgs<McEntity>> bumpHandler = (_, __) => bumped = true;
            mcParent.Bumped += bumpHandler;
            try
            {
                if (TryGetDirectionAndMove(map, mcParent))
                {
                    return bumped
                        ? (true, TimeHelper.GetAttackTime(mcParent))
                        : (true, TimeHelper.GetWalkTime(mcParent));
                }
                else
                {
                    return (true, TimeHelper.Wait);
                }
            }
            finally
            {
                mcParent.Bumped -= bumpHandler;
            }
        }

        public bool TryGetDirectionAndMove(DungeonMap map, McEntity mcParent)
        {
            var path = map.AStar.ShortestPath(Parent.Position, map.Player.Position);

            Direction direction;
            if (path == null || path.Length > _range)
            {
                return false;
            }
            else
            {
                direction = Direction.GetDirection(path.Steps.First() - Parent.Position);
            }

            mcParent.Move(direction);
            return true;
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

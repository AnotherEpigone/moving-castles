using GoRogue;
using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Serialization;

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

        public bool Run(DungeonMap map, ILogManager logManager)
        {
            if (!(Parent is McEntity mcParent))
            {
                return false;
            }

            var walkSpeed = mcParent.GetGoRogueComponent<IActorStatComponent>()?.WalkSpeed ?? 1;

            // if we bump into something, stop moving.
            // walk speed doesn't allow you to attack or interact more than once.
            var bumped = false;
            mcParent.Bumped += (_, __) => bumped = true;
            for ( int i = 0; i < walkSpeed; i++)
            {
                if (!TryGetDirectionAndMove(map, mcParent))
                {
                    return false;
                }

                if (bumped)
                {
                    break;
                }
            }

            return true;
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

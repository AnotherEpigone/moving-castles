using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Components.AiComponents
{
    /// <summary>
    /// AI component that runs through a set of child AI components until one succeeds.
    /// </summary>
    public class LinearCompositeAiComponent : IAiComponent, ISerializableComponent
    {
        private readonly List<IAiComponent> _components;
        private IGameObject _parent;

        public LinearCompositeAiComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _components = stateObj.Components
                .Select(sc => ComponentFactory.Create(sc))
                .Cast<IAiComponent>()
                .ToList();
        }

        public LinearCompositeAiComponent(params IAiComponent[] components)
        {
            _components = new List<IAiComponent>(components);
        }

        public IGameObject Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                foreach (var component in _components)
                {
                    component.Parent = Parent;
                }
            }
        }

        public bool Run(DungeonMap map, ILogManager logManager)
        {
            foreach (var component in _components)
            {
                if (component.Run(map, logManager))
                {
                    return true;
                }
            }

            return false;
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(LinearCompositeAiComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                Components = _components.ConvertAll(c => ((ISerializableComponent)c).GetSerializable()),
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public List<ComponentSerializable> Components;
        }
    }
}

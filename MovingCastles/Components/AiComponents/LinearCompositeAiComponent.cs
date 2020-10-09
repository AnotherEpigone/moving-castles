using GoRogue.GameFramework;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using System.Collections.Generic;

namespace MovingCastles.Components.AiComponents
{
    /// <summary>
    /// AI component that runs through a set of child AI components until one succeeds.
    /// </summary>
    public class LinearCompositeAiComponent : IAiComponent
    {
        private readonly List<IAiComponent> _components;
        private IGameObject _parent;

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
    }
}

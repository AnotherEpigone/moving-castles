using GoRogue.GameFramework.Components;
using System;
using System.Collections.Generic;

namespace MovingCastles.Components.Serialization
{
    public static class ComponentFactory
    {
        private static readonly Dictionary<string, Func<string, IGameObjectComponent>> _constructors;

        static ComponentFactory()
        {
            _constructors = new Dictionary<string, Func<string, IGameObjectComponent>>()
            {
                { nameof(OpenDoorComponent), _ => new OpenDoorComponent() },
            };
        }

        public static IGameObjectComponent Create(ComponentSerializable serialized)
        {
            if (!_constructors.TryGetValue(serialized.Id, out var constructor))
            {
                throw new NotSupportedException($"Unsupported component id: {serialized.Id}");
            }

            return constructor(serialized.State);
        }
    }
}

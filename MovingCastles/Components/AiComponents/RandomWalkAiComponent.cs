using GoRogue;
using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Stats;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using Troschuetz.Random;

namespace MovingCastles.Components.AiComponents
{
    public class RandomWalkAiComponent : IAiComponent, ISerializableComponent
    {
        public IGameObject Parent { get; set; }

        private readonly float _restChance;

        public RandomWalkAiComponent()
            : this(0.5f) { }

        public RandomWalkAiComponent(float restChance)
        {
            _restChance = restChance;
        }

        public bool Run(DungeonMap map, IGenerator rng, ILogManager logManager)
        {
            if (Parent is not McEntity mcParent)
            {
                return false;
            }

            if (rng.NextDouble() < _restChance)
            {
                mcParent.GetGoRogueComponent<IHealthComponent>()?.ApplyBaseRegen();
                return true;
            }

            var directionType = rng.Next(0, 8);
            var direction = Direction.ToDirection((Direction.Types)directionType);
            mcParent.Move(direction);

            return true;
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(RandomWalkAiComponent),
            };
        }
    }
}

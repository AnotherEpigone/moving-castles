using GoRogue;
using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Time;
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

        public (bool success, int ticks) Run(McMap map, IGenerator rng, ILogManager logManager)
        {
            if (Parent is not McEntity mcParent)
            {
                return (false, -1);
            }

            if (rng.NextDouble() < _restChance)
            {
                return (true, TimeHelper.Wait);
            }

            var directionType = rng.Next(0, 8);
            var direction = Direction.ToDirection((Direction.Types)directionType);
            var outcome = mcParent.Move(direction);
            return outcome switch
            {
                MoveOutcome.Move => (true, TimeHelper.GetWalkTime(mcParent)),
                MoveOutcome.NoMove => (true, TimeHelper.Wait),
                MoveOutcome.Melee => (true, TimeHelper.GetAttackTime(mcParent)),
                _ => throw new System.NotSupportedException($"Unsupported move outcome {outcome}."),
            };
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

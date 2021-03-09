using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Stats;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Time;
using MovingCastles.Maps;
using Troschuetz.Random;

namespace MovingCastles.Components.AiComponents
{
    public class RangedAttackAiComponent : IAiComponent, ISerializableComponent
    {
        public IGameObject Parent { get; set; }

        public (bool success, int ticks) Run(McMap map, IGenerator rng, ILogManager logManager)
        {
            if (Parent is not McEntity mcParent)
            {
                return (false, -1);
            }

            var rangedAttackComponent = mcParent.GetGoRogueComponent<IRangedAttackerComponent>();
            return rangedAttackComponent.TryAttack(map, rng, logManager)
                ? (true, TimeHelper.GetAttackTime(mcParent))
                : (false, -1);
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(RangedAttackAiComponent),
            };
        }
    }
}

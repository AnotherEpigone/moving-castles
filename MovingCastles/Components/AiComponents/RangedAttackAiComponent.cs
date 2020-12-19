using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using Troschuetz.Random;

namespace MovingCastles.Components.AiComponents
{
    public class RangedAttackAiComponent : IAiComponent, ISerializableComponent
    {
        public IGameObject Parent { get; set; }

        public bool Run(DungeonMap map, IGenerator rng, ILogManager logManager)
        {
            if (Parent is not McEntity mcParent)
            {
                return false;
            }

            var rangedAttackComponent = mcParent.GetGoRogueComponent<IRangedAttackerComponent>();
            return rangedAttackComponent.TryAttack(map, rng, logManager);
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

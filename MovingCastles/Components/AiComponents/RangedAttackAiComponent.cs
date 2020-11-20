using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;

namespace MovingCastles.Components.AiComponents
{
    public class RangedAttackAiComponent : IAiComponent, ISerializableComponent
    {
        public IGameObject Parent { get; set; }

        public bool Run(DungeonMap map, ILogManager logManager)
        {
            if (!(Parent is McEntity mcParent))
            {
                return false;
            }

            var rangedAttackComponent = mcParent.GetGoRogueComponent<IRangedAttackerComponent>();
            return rangedAttackComponent.TryAttack(map, logManager);
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

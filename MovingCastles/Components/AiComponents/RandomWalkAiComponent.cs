using GoRogue;
using GoRogue.GameFramework;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
namespace MovingCastles.Components.AiComponents
{
    public class RandomWalkAiComponent : IAiComponent
    {
        public IGameObject Parent { get; set; }

        private readonly float _restChance;

        public RandomWalkAiComponent()
            : this(0.5f) { }

        public RandomWalkAiComponent(float restChance)
        {
            _restChance = restChance;
        }

        public bool Run(DungeonMap map, ILogManager logManager)
        {
            if (!(Parent is McEntity mcParent))
            {
                return false;
            }

            if (SadConsole.Global.Random.NextDouble() < _restChance)
            {
                mcParent.GetGoRogueComponent<IHealthComponent>()?.ApplyBaseRegen();
                return true;
            }

            var directionType = SadConsole.Global.Random.Next(0, 8);
            var direction = Direction.ToDirection((Direction.Types)directionType);
            mcParent.Move(direction);

            return true;
        }
    }
}

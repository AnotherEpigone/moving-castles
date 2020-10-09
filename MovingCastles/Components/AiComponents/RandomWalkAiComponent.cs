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

        public bool Run(DungeonMap map, ILogManager logManager)
        {
            var directionType = SadConsole.Global.Random.Next(0, 8);
            var direction = Direction.ToDirection((Direction.Types)directionType);

            if (Parent is McEntity mcParent)
            {
                mcParent.Move(direction);
            }
            else
            {
                Parent.Position += direction;
            }

            return true;
        }
    }
}

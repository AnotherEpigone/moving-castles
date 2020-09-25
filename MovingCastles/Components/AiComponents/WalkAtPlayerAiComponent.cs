using GoRogue;
using GoRogue.GameFramework;
using MovingCastles.Entities;
using MovingCastles.Maps;
using System.Linq;

namespace MovingCastles.Components.AiComponents
{
    public class WalkAtPlayerAiComponent : IAiComponent
    {
        private readonly int _range;

        public WalkAtPlayerAiComponent(int range)
        {
            _range = range;
        }

        public IGameObject Parent { get; set; }

        public void Run(MovingCastlesMap map)
        {
            if (!(Parent is McEntity mcParent))
            {
                return;
            }

            var path = map.AStar.ShortestPath(Parent.Position, map.Player.Position);

            Direction direction;
            if (path == null || path.Length > _range)
            {
                // can't reach player or player is far away, move randomly
                var directionType = SadConsole.Global.Random.Next(0, 8);
                direction = Direction.ToDirection((Direction.Types)directionType);
            }
            else
            {
                direction = Direction.GetDirection(path.Steps.First() - Parent.Position);
            }

            mcParent.Move(direction);
        }
    }
}

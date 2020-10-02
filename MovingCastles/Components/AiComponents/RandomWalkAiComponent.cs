using GoRogue;
using GoRogue.GameFramework;
using MovingCastles.Entities;
using MovingCastles.Maps;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovingCastles.Components.AiComponents
{
    public class RandomWalkAiComponent : IAiComponent
    {
        public IGameObject Parent { get; set; }

        public void Run(DungeonMap map)
        {
            var directionType = SadConsole.Global.Random.Next(0, 8);
            var direction = Direction.ToDirection((Direction.Types)directionType);

            if (!(Parent is McEntity mcParent))
            {
                Parent.Position += direction;
                return;
            }

            mcParent.Move(direction);
        }
    }
}

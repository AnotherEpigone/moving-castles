using GoRogue.GameFramework.Components;
using MovingCastles.Maps;

namespace MovingCastles.Components.AiComponents
{
    public interface IAiComponent : IGameObjectComponent
    {
        void Run(MovingCastlesMap map);
    }
}

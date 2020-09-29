using MovingCastles.Maps;
using SadConsole;

namespace MovingCastles.Ui
{
    public interface IUiManager
    {
        int ViewPortHeight { get; }
        int ViewPortWidth { get; }

        Console CreateMainMenu();
        ContainerConsole CreateMapScreen(IMapPlan mapPlan);
    }
}
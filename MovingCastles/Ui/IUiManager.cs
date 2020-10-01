using MovingCastles.GameSystems;
using MovingCastles.Maps;
using SadConsole;

namespace MovingCastles.Ui
{
    public interface IUiManager
    {
        int ViewPortHeight { get; }
        int ViewPortWidth { get; }

        void ShowMainMenu(IGameManager gameManager);
        ContainerConsole CreateMapScreen(IMapPlan mapPlan, IGameManager gameManager);
    }
}
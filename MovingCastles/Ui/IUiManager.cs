using MovingCastles.GameSystems;
using MovingCastles.Maps;
using MovingCastles.Ui.Consoles;
using SadConsole;

namespace MovingCastles.Ui
{
    public interface IUiManager
    {
        int ViewPortHeight { get; }
        int ViewPortWidth { get; }

        void ShowMainMenu(IGameManager gameManager);
        ContainerConsole CreateDungeonMapScreen(IMapPlan mapPlan, IGameManager gameManager);
        CastleModeConsole CreateCastleMapScreen(IMapPlan mapPlan, IGameManager gameManager);
    }
}
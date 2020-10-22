using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Player;
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
        ContainerConsole CreateDungeonMapScreen(
            IGameManager gameManager,
            DungeonMap map,
            Font tilesetFont);
        CastleModeConsole CreateCastleMapScreen(
            IGameManager gameManager,
            CastleMap map,
            Font tilesetFont);
    }
}
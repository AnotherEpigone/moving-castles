using MovingCastles.GameSystems;
using MovingCastles.GameSystems.TurnBased;
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

        MainConsole CreateDungeonMapScreen(
            IGameManager gameManager,
            ITurnBasedGame game,
            ITurnBasedGameConsoleFactory turnBasedGameConsoleFactory,
            IDungeonMaster dungeonMaster,
            Font tilesetFont);

        CastleModeConsole CreateCastleMapScreen(
            IGameManager gameManager,
            McMap map,
            Font tilesetFont);

        void ToggleFullScreen();

        void SetViewport(int width, int height);
    }
}
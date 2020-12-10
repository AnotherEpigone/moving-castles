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

        DungeonModeConsole CreateDungeonMapScreen(
            IGameManager gameManager,
            ITurnBasedGame turnBasedGame,
            DungeonMap map,
            Font tilesetFont);

        CastleModeConsole CreateCastleMapScreen(
            IGameManager gameManager,
            CastleMap map,
            Font tilesetFont);

        void ActivateFullScreen();
    }
}
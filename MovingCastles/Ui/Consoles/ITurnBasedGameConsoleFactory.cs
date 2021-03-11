using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using MovingCastles.Serialization.Settings;
using SadConsole;

namespace MovingCastles.Ui.Consoles
{
    public interface ITurnBasedGameConsoleFactory
    {
        ITurnBasedGameConsole Create(
            int x,
            int y,
            int width,
            int height,
            Font font,
            IMapModeMenuProvider menuProvider,
            ITurnBasedGame game,
            IAppSettings appSettings,
            McMap map);
    }
}

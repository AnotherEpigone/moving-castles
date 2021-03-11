using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using MovingCastles.Serialization.Settings;
using SadConsole;

namespace MovingCastles.Ui.Consoles
{
    public class CastleMapConsoleFactory : ITurnBasedGameConsoleFactory
    {
        public ITurnBasedGameConsole Create(int x, int y, int width, int height, Font font, IMapModeMenuProvider menuProvider, ITurnBasedGame game, IAppSettings appSettings, McMap map)
        {
            return new CastleMapConsole(
                width,
                height,
                font,
                menuProvider,
                map)
            {
                Position = new Microsoft.Xna.Framework.Point(x, y),
            };
        }
    }
}

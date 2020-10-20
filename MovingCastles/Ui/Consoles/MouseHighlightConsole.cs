using GoRogue;
using MovingCastles.GameSystems.TurnBasedGame;
using MovingCastles.Maps;
using SadConsole;
using SadConsole.Input;

namespace MovingCastles.Ui.Consoles
{
    public class MouseHighlightConsole : Console
    {
        private readonly ITurnBasedGame _game;
        private readonly DungeonMap _map;

        public MouseHighlightConsole(
            int width,
            int height,
            Font font,
            ITurnBasedGame game,
            DungeonMap map)
            : base(width, height, font)
        {
            UseMouse = false;
            _game = game;
            _map = map;
        }

        public void Draw(MouseConsoleState state, Coord mapCoord)
        {
            IsVisible = state.IsOnConsole && _map.Explored[mapCoord];
            if (!IsVisible)
            {
                return;
            }

            Clear();
            
            var mousePos = state.ConsoleCellPosition;
            if (_game.State == State.PlayerTurn)
            {
                SetGlyph(mousePos.X, mousePos.Y, 1, ColorHelper.WhiteHighlight);
                return;
            }

            if (_game.State != State.Targetting)
            {
                return;
            }

            var highlightColor = _game.TargettingSpell.TargettingStyle.Offensive
                ? ColorHelper.RedHighlight
                : ColorHelper.YellowHighlight;
            SetGlyph(mousePos.X, mousePos.Y, 1, highlightColor);
        }
    }
}

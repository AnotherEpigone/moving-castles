using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.GameSystems.Spells;
using MovingCastles.GameSystems.TurnBased;
using MovingCastles.Maps;
using SadConsole;
using System.Linq;

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

        public void Draw(Point consolePos, Point mapPos, bool positionIsTargetable)
        {
            if (!IsVisible)
            {
                return;
            }

            Clear();

            var mousePos = consolePos;
            switch (_game.State)
            {
                case State.PlayerTurn:
                    SetGlyph(mousePos.X, mousePos.Y, 1, ColorHelper.WhiteHighlight);
                    break;
                case State.Targetting:
                    DrawTargettingMode(mousePos, _map.Player.Position, mapPos, _game.TargettingSpell.TargettingStyle, positionIsTargetable);
                    break;
                case State.Processing:
                default:
                    break;
            }
        }

        private void DrawTargettingMode(Point mousePos, Point playerPos, Point mapPos, ITargettingStyle targettingStyle, bool positionIsTargetable)
        {
            if (!positionIsTargetable)
            {
                return;
            }

            switch (targettingStyle.TargetMode)
            {
                case TargetMode.SingleTarget:
                    DrawSingleTarget(mousePos, playerPos, mapPos, targettingStyle);
                    break;
                case TargetMode.Projectile:
                    DrawProjectile(mousePos, playerPos, mapPos, targettingStyle);
                    break;
                default:
                    break;
            }
        }

        private void DrawSingleTarget(Point mousePos, Point playerPos, Point mapPos, ITargettingStyle targettingStyle)
        {
            var mouseMapPos = mousePos + mapPos;
            var distance = Distance.CHEBYSHEV.Calculate(playerPos, mouseMapPos);
            if (distance > targettingStyle.Range)
            {
                SetGlyph(mousePos.X, mousePos.Y, 1, ColorHelper.DarkGreyHighlight);
                return;
            }

            var highlightColor = targettingStyle.Offensive
                ? ColorHelper.RedHighlight
                : ColorHelper.YellowHighlight;
            SetGlyph(mousePos.X, mousePos.Y, 1, highlightColor);
        }

        private void DrawProjectile(Point mousePos, Point playerPos, Point mapPos, ITargettingStyle targettingStyle)
        {
            var mouseMapPos = mousePos + mapPos;
            var distance = Distance.CHEBYSHEV.Calculate(playerPos, mouseMapPos);
            if (distance > targettingStyle.Range)
            {
                SetGlyph(mousePos.X, mousePos.Y, 1, ColorHelper.DarkGreyHighlight);
                return;
            }

            var highlightColor = targettingStyle.Offensive
                ? ColorHelper.RedHighlight
                : ColorHelper.YellowHighlight;
            var line = Lines.Get(playerPos, mouseMapPos, Lines.Algorithm.DDA);
            foreach (var point in line.Skip(1))
            {
                var pointConsolePos = point - mapPos;
                SetGlyph(pointConsolePos.X, pointConsolePos.Y, 1, highlightColor);

                if (!_map.WalkabilityView[point])
                {
                    break;
                }
            }
        }
    }
}

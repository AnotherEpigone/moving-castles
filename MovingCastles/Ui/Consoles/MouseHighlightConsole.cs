﻿using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.GameSystems.Spells;
using MovingCastles.GameSystems.TurnBasedGame;
using MovingCastles.Maps;
using SadConsole;
using SadConsole.Input;
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

        public void Draw(MouseConsoleState state, Point mapPos)
        {
            if (!IsVisible)
            {
                return;
            }

            Clear();

            var mousePos = state.ConsoleCellPosition;
            switch (_game.State)
            {
                case State.PlayerTurn:
                    SetGlyph(mousePos.X, mousePos.Y, 1, ColorHelper.WhiteHighlight);
                    break;
                case State.Targetting:
                    DrawTargettingMode(mousePos, _map.Player.Position, mapPos, _game.TargettingSpell.TargettingStyle);
                    break;
                case State.Processing:
                default:
                    break;
            }
        }

        private void DrawTargettingMode(Point mousePos, Point playerPos, Point mapPos, ITargettingStyle targettingStyle)
        {
            var highlightColor = targettingStyle.Offensive
                ? ColorHelper.RedHighlight
                : ColorHelper.YellowHighlight;

            switch (targettingStyle.TargetMode)
            {
                case TargetMode.SingleTarget:
                    SetGlyph(mousePos.X, mousePos.Y, 1, highlightColor);
                    break;
                case TargetMode.Projectile:
                    DrawProjectile(mousePos, playerPos, mapPos, highlightColor);
                    break;
                default:
                    break;
            }
        }

        private void DrawProjectile(Point mousePos, Point playerPos, Point mapPos, Color highlightColor)
        {
            var mouseMapPos = mousePos + mapPos;
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

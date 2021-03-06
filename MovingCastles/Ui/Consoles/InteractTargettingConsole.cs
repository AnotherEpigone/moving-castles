﻿using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Fonts;
using MovingCastles.Maps;
using SadConsole;
using System.Collections.Generic;
using System.Diagnostics;

namespace MovingCastles.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class InteractTargettingConsole : Console
    {
        private readonly List<(Coord, int)> _sprites;
        private readonly McMap _map;

        public InteractTargettingConsole(Font font, McMap map)
            : base(3, 3, font)
        {
            UseMouse = false;
            _map = map;
            _sprites = new List<(Coord, int)>
            {
                { (new Coord(-1, -1), DungeonModeSpriteAtlas.BlueArrow_UpLeft) },
                { (new Coord(0, -1), DungeonModeSpriteAtlas.BlueArrow_Up) },
                { (new Coord(1, -1), DungeonModeSpriteAtlas.BlueArrow_UpRight) },
                { (new Coord(1, 0), DungeonModeSpriteAtlas.BlueArrow_Right) },
                { (new Coord(1, 1), DungeonModeSpriteAtlas.BlueArrow_DownRight) },
                { (new Coord(0, 1), DungeonModeSpriteAtlas.BlueArrow_Down) },
                { (new Coord(-1, 1), DungeonModeSpriteAtlas.BlueArrow_DownLeft) },
                { (new Coord(-1, 0), DungeonModeSpriteAtlas.BlueArrow_Left) },
                { (new Coord(0, 0), 0) },
            };
        }

        public void Draw(Coord position, List<Coord> interactablePositions)
        {
            if (!IsVisible)
            {
                return;
            }

            Clear();

            Position = new Coord(position.X - 1, position.Y - 1);

            var center = new Coord(1, 1);
            foreach (var (offset, glyph) in _sprites)
            {
                var overlayPos = center + offset;
                var mapPos = position + offset;
                if (interactablePositions.Contains(mapPos))
                {
                    SetGlyph(overlayPos.X, overlayPos.Y, glyph, Color.White, ColorHelper.WhiteHighlight);
                }
                else
                {
                    SetGlyph(overlayPos.X, overlayPos.Y, glyph, ColorHelper.GreyHighlight);
                }
            }
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(InteractTargettingConsole)} ({Position.X}, {Position.Y})");
            }
        }
    }
}

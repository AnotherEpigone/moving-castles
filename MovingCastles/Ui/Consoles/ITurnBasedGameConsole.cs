using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using MovingCastles.Maps;
using SadConsole;
using System;

namespace MovingCastles.Ui.Consoles
{
    public interface ITurnBasedGameConsole : IDisposable
    {
        event EventHandler<string> FlavorMessageChanged;

        int ViewportWidth { get; }

        int ViewportHeight { get; }

        Font Font { get; }

        SadConsole.Console ThisConsole { get; }

        Wizard Player { get; }

        Color DefaultBackground { get; }

        Point Position { get; set; }

        void StartTargetting(SpellTemplate spell);

        void SetMap(McMap map, Font tilesetFont);

        void UnsetMap();
    }
}

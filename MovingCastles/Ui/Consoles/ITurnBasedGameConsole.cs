using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using MovingCastles.Maps;
using System;

namespace MovingCastles.Ui.Consoles
{
    public interface ITurnBasedGameConsole
    {
        event EventHandler<string> FlavorMessageChanged;

        SadConsole.Console ThisConsole { get; }

        Wizard Player { get; }

        Color DefaultBackground { get; }

        Point Position { get; set; }

        void StartTargetting(SpellTemplate spell);

        void SetMap(McMap map);

        void UnsetMap();
    }
}

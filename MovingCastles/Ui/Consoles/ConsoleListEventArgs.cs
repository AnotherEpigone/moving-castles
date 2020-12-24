using GoRogue;
using SadConsole;
using System.Collections.Generic;

namespace MovingCastles.Ui.Consoles
{
    public class ConsoleListEventArgs : System.EventArgs
    {
        public ConsoleListEventArgs(List<Console> consoles, Coord pixelPosition)
        {
            Consoles = consoles;
            PixelPosition = pixelPosition;
        }

        public List<Console> Consoles { get; }

        public Coord PixelPosition { get; }
    }
}

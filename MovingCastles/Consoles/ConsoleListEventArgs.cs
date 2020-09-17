using SadConsole;
using System.Collections.Generic;

namespace MovingCastles.Consoles
{
    public class ConsoleListEventArgs : System.EventArgs
    {
        public ConsoleListEventArgs(List<Console> consoles)
        {
            Consoles = consoles;
        }

        public List<Console> Consoles { get; }
    }
}

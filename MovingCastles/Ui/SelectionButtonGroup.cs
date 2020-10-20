using SadConsole.Controls;
using System.Collections.Generic;

namespace MovingCastles.Ui
{
    public class SelectionButtonGroup
    {
        private readonly List<SelectionButton> _selectionButtons;

        public SelectionButtonGroup(IEnumerable<SelectionButton> selectionButtons)
        {
            _selectionButtons = new List<SelectionButton>(selectionButtons);
        }
    }
}

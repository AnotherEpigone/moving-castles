using SadConsole;
using SadConsole.Controls;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Ui.Consoles
{
    public class McControlsConsole : ControlsConsole
    {
        public McControlsConsole(int width, int height)
            : base(width, height) { }

        private Dictionary<SelectionButton, System.Action> _selectionButtons;
        private SelectionButton _lastFocusedButton;

        public void SetupSelectionButtons(params SelectionButton[] buttons)
        {
            SetupSelectionButtons(new Dictionary<SelectionButton, System.Action>(buttons.Select(b => new KeyValuePair<SelectionButton, System.Action>(b, () => { }))));
        }

        public void SetupSelectionButtons(Dictionary<SelectionButton, System.Action> buttonSelectionActions)
        {
            _selectionButtons = new Dictionary<SelectionButton, System.Action>(buttonSelectionActions);
            if (_selectionButtons.Count < 1)
            {
                return;
            }

            var buttons = buttonSelectionActions.Keys.ToArray();
            for (int i = 1; i < _selectionButtons.Count; i++)
            {
                buttons[i - 1].NextSelection = buttons[i];
                buttons[i].PreviousSelection = buttons[i - 1];
            }

            buttons[0].PreviousSelection = buttons[_selectionButtons.Count - 1];
            buttons[_selectionButtons.Count - 1].NextSelection = buttons[0];

            foreach (var button in buttons)
            {
                Add(button);
                button.MouseEnter += (_, __) =>
                {
                    FocusedControl = button;
                };
            }

            FocusedControl = buttons[0];
        }

        public override void Update(System.TimeSpan time)
        {
            if (!(FocusedControl is SelectionButton focusedButton)
                || focusedButton == _lastFocusedButton)
            {
                base.Update(time);
                return;
            }

            _lastFocusedButton = focusedButton;
            _selectionButtons[focusedButton]();

            base.Update(time);
        }
    }
}

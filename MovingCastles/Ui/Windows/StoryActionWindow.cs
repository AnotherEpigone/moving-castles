using Microsoft.Xna.Framework;
using MovingCastles.GameSystems;
using SadConsole.Controls;
using SadConsole.Themes;

namespace MovingCastles.Ui.Windows
{
    public class StoryActionWindow : McControlWindow
    {
        private readonly SadConsole.Console _storyArea;
        private readonly IScenarioMaster _scenarioMaster;

        public StoryActionWindow(
            int width,
            int height,
            IScenarioMaster scenarioMaster)
            : base(width, height)
        {
            CloseOnEscKey = false;
            IsModalDefault = true;
            Center();

            IsFocused = true;

            _scenarioMaster = scenarioMaster;

            _storyArea = new SadConsole.Console(Width - 2, Height - 20)
            {
                Position = new Point(1, 1),
                DefaultBackground = ColorHelper.ControlBackDark,
            };

            _storyArea.Fill(null, ColorHelper.ControlBackDark, null);

            Children.Add(_storyArea);

            var option1Button = new Button(Width - 2)
            {
                Text = "Option 1",
                Position = new Point(1, Height - 18),
                TextAlignment = SadConsole.HorizontalAlignment.Left,
            };
            option1Button.Click += (_, __) => _scenarioMaster.Hide();

            var actionTheme = (ButtonTheme)option1Button.Theme;
            actionTheme.ShowEnds = false;
            option1Button.Theme = actionTheme;

            Add(option1Button);
        }
    }
}

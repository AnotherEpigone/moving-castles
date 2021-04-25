using Microsoft.Xna.Framework;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Scenarios;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using System.Collections.Generic;

namespace MovingCastles.Ui.Windows
{
    public class ScenarioWindow : McControlWindow
    {
        private readonly SadConsole.Console _storyArea;

        private readonly IDungeonMaster _dungeonMaster;
        private readonly ILogManager _logManager;

        private readonly ButtonTheme _actionTheme;

        public ScenarioWindow(
            int width,
            int height,
            IScenarioStep firstStep,
            IDungeonMaster dungeonMaster,
            ILogManager logManager)
            : base(width, height)
        {
            CloseOnEscKey = false;
            IsModalDefault = true;
            Center();

            IsFocused = true;

            _dungeonMaster = dungeonMaster;
            _logManager = logManager;

            _storyArea = new SadConsole.Console(Width - 2, Height - 20)
            {
                Position = new Point(1, 1),
                DefaultBackground = ColorHelper.ControlBackDark,
            };
            _storyArea.Fill(null, ColorHelper.ControlBackDark, null);

            Children.Add(_storyArea);

            _actionTheme = (ButtonTheme)new Button(1).Theme;
            _actionTheme.ShowEnds = false;

            SetupStep(firstStep);
        }

        private void SetupStep(IScenarioStep step)
        {
            _storyArea.Clear();

            var coloredDescription = new ColoredString(step.Description, new Cell(Color.Gainsboro, ColorHelper.ControlBackDark));
            _storyArea.Cursor.Print(coloredDescription);

            RemoveAll();
            var buttonY = Height - 16;
            foreach (var action in step.Actions)
            {
                buttonY -= 2;
                var button = new Button(Width - 2)
                {
                    Text = action.Description,
                    Position = new Point(1, buttonY),
                    TextAlignment = SadConsole.HorizontalAlignment.Left,
                    Theme = _actionTheme,
                };
                button.Click += (_, __) =>
                {
                    action.SelectAction(_dungeonMaster, _logManager);
                    action.NextStep.Match(
                        nextStep => SetupStep(nextStep),
                        () => _dungeonMaster.ScenarioMaster.Hide());
                };

                Add(button);
            }
            
        }
    }
}

using Microsoft.Xna.Framework;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Scenarios;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using System;
using System.Collections.Generic;

namespace MovingCastles.Ui.Windows
{
    public class ScenarioWindow : McControlWindow
    {
        private readonly SadConsole.Console _storyArea;

        private readonly IDungeonMaster _dungeonMaster;
        private readonly ILogManager _logManager;

        private readonly ButtonTheme _actionTheme;

        private readonly Dictionary<char, ScenarioStepAction> _hotkeys;

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
            _hotkeys = new Dictionary<char, ScenarioStepAction>();

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

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            foreach (var key in info.KeysPressed)
            {
                if (_hotkeys.TryGetValue(key.Character, out var action))
                {
                    InvokeAction(action);
                    return true;
                }
            }

            return base.ProcessKeyboard(info);
        }

        private void InvokeAction(ScenarioStepAction action)
        {
            action.SelectAction(_dungeonMaster, _logManager);
            action.NextStep.Match(
                nextStep => SetupStep(nextStep),
                () => _dungeonMaster.ScenarioMaster.Hide());
        }

        private void SetupStep(IScenarioStep step)
        {
            _storyArea.Clear();

            var coloredDescription = new ColoredString(step.Description, new Cell(Color.Gainsboro, ColorHelper.ControlBackDark));
            _storyArea.Cursor.Position = new Point(0, 1);
            _storyArea.Cursor.Print(coloredDescription);

            RemoveAll();
            var buttonY = Height - 20;
            var hotkeyCount = 0;
            foreach (var action in step.Actions)
            {
                var hotkeyLetter = (char)('a' + hotkeyCount);
                _hotkeys.Add(hotkeyLetter, action);

                buttonY += 2;
                hotkeyCount++;
                var button = new Button(Width - 2)
                {
                    Text = $"{System.Char.ToUpper(hotkeyLetter)}. {action.Description}",
                    Position = new Point(1, buttonY),
                    TextAlignment = HorizontalAlignment.Left,
                    Theme = _actionTheme,
                };
                button.Click += (_, __) => InvokeAction(action);

                Add(button);
            }
            
        }
    }
}

using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Scenarios;
using MovingCastles.Ui.Consoles;
using MovingCastles.Ui.Windows;
using SadConsole;
using Troschuetz.Random;

namespace MovingCastles.GameSystems
{
    public class ScenarioMaster : IScenarioMaster
    {
        private ScenarioWindow _window;
        private ITurnBasedGameConsole _mapConsole;

        public void Show(
            IScenario scenario,
            IDungeonMaster dungeonMaster,
            ILogManager logManager,
            IGenerator rng)
        {
            var firstStep = scenario.Encounter(dungeonMaster, rng);

            _mapConsole = ((MainConsole)Global.CurrentScreen).MapConsole;

            float widthFactor = _mapConsole.Font.Size.X / (float)Global.FontDefault.Size.X;
            float heightFactor = _mapConsole.Font.Size.Y / (float)Global.FontDefault.Size.Y;
            _window = new ScenarioWindow(
                (int)(_mapConsole.ViewportWidth * widthFactor) - 32,
                (int)(_mapConsole.ViewportHeight * heightFactor) - 8,
                firstStep,
                dungeonMaster,
                logManager);
            _window.Show();
        }

        public void Hide()
        {
            _window.Hide();
            ((Console)_mapConsole).IsFocused = true;
        }
    }
}

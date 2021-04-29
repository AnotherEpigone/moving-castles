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

        public void Show(
            IScenario scenario,
            IDungeonMaster dungeonMaster,
            ILogManager logManager,
            IGenerator rng)
        {
            var firstStep = scenario.Encounter(dungeonMaster, rng);

            var mapConsole = ((MainConsole)Global.CurrentScreen).MapConsole;

            float widthFactor = mapConsole.Font.Size.X / (float)Global.FontDefault.Size.X;
            float heightFactor = mapConsole.Font.Size.Y / (float)Global.FontDefault.Size.Y;
            _window = new ScenarioWindow(
                (int)(mapConsole.ViewportWidth * widthFactor) - 32,
                (int)(mapConsole.ViewportHeight * heightFactor) - 8,
                firstStep,
                dungeonMaster,
                logManager);
            _window.Show();
        }

        public void Hide()
        {
            _window.Hide();
            var mapConsole = (Console)((MainConsole)Global.CurrentScreen).MapConsole;
            mapConsole.IsFocused = true;
        }
    }
}

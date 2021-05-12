using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Scenarios;
using MovingCastles.Ui;
using MovingCastles.Ui.Consoles;
using MovingCastles.Ui.Windows;
using SadConsole;
using Troschuetz.Random;

namespace MovingCastles.GameSystems
{
    public class ScenarioMaster : IScenarioMaster
    {
        private ScenarioWindow _window;
        private readonly IUiManager _uiManager;

        public ScenarioMaster(IUiManager uiManager)
        {
            _uiManager = uiManager;
        }

        public void Show(
            IScenario scenario,
            IDungeonMaster dungeonMaster,
            ILogManager logManager,
            IGenerator rng)
        {
            var firstStep = scenario.Encounter(dungeonMaster, rng);

            var mapConsoleSize = _uiManager.GetCentralWindowSize();
            _window = new ScenarioWindow(
                mapConsoleSize.X,
                mapConsoleSize.Y,
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

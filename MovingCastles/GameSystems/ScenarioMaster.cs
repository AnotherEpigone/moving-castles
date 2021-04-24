using MovingCastles.GameSystems.Scenarios;
using MovingCastles.Ui.Consoles;
using MovingCastles.Ui.Windows;
using SadConsole;
using Troschuetz.Random;

namespace MovingCastles.GameSystems
{
    public class ScenarioMaster : IScenarioMaster
    {
        private StoryActionWindow _window;
        private Console _mapConsole;

        public void Show(SimpleScenario scenario, IDungeonMaster dungeonMaster, IGenerator rng)
        {
            scenario.Encounter(dungeonMaster, rng);

            _mapConsole = (Console)((MainConsole)Global.CurrentScreen).MapConsole;
            _window = new StoryActionWindow(_mapConsole.Width, _mapConsole.Height, this);
            _window.Show();
        }

        public void Hide()
        {
            _window.Hide();
            _mapConsole.IsFocused = true;
        }
    }
}

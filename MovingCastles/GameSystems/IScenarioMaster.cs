using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Scenarios;
using Troschuetz.Random;

namespace MovingCastles.GameSystems
{
    public interface IScenarioMaster
    {
        void Show(
            IScenario scenario,
            IDungeonMaster dungeonMaster,
            ILogManager logManager,
            IGenerator rng);

        void Hide();
    }
}
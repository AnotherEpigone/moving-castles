using MovingCastles.GameSystems.Scenarios;
using Troschuetz.Random;

namespace MovingCastles.GameSystems
{
    public interface IScenarioMaster
    {
        void Show(SimpleScenario scenario, IDungeonMaster dungeonMaster, IGenerator rng);

        void Hide();
    }
}
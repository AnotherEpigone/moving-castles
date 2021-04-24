using Troschuetz.Random;

namespace MovingCastles.GameSystems.Scenarios
{
    public class SimpleScenario : IScenario
    {
        private readonly IScenarioStep _firstStep;

        public SimpleScenario(string id, IScenarioStep firstStep)
        {
            _firstStep = firstStep;
            Id = id;
        }

        public string Id { get; }

        public IScenarioStep Encounter(IDungeonMaster dungeonMaster, IGenerator rng)
        {
            return _firstStep;
        }
    }
}

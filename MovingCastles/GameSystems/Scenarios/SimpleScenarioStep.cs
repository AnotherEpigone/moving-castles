using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.GameSystems.Scenarios
{
    public class SimpleScenarioStep : IScenarioStep
    {
        public SimpleScenarioStep(
            string description,
            IEnumerable<ScenarioStepAction> actions)
        {
            Description = description;
            Actions = actions.ToList();
        }

        public string Description { get; }

        public IList<ScenarioStepAction> Actions { get; }
    }
}

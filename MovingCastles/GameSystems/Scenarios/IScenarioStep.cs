using System.Collections.Generic;

namespace MovingCastles.GameSystems.Scenarios
{
    public interface IScenarioStep
    {
        IList<ScenarioStepAction> Actions { get; }
        string Description { get; }
    }
}
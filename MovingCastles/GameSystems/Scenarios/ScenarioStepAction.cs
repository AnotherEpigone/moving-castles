using MovingCastles.GameSystems.Logging;
using Optional;
using System;

namespace MovingCastles.GameSystems.Scenarios
{
    public class ScenarioStepAction
    {
        public ScenarioStepAction(
            string description,
            Action<IDungeonMaster, ILogManager> selectAction,
            Option<SimpleScenarioStep> nextStep)
        {
            Description = description;
            SelectAction = selectAction;
            NextStep = nextStep;
        }

        public static ScenarioStepAction End(string description) => new ScenarioStepAction(
                description,
                (_, __) => { },
                Option.None<SimpleScenarioStep>());

        public string Description { get; }

        public Action<IDungeonMaster, ILogManager> SelectAction { get; }

        public Option<SimpleScenarioStep> NextStep { get; }
    }
}

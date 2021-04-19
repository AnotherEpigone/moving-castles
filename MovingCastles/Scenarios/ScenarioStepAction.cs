using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using Optional;
using System;

namespace MovingCastles.Scenarios
{
    public class ScenarioStepAction
    {
        public string Description { get; }

        public Action<IDungeonMaster, ILogManager> SelectAction { get; }

        public Option<ScenarioStep> NextStep { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovingCastles.GameSystems.Scenarios
{
    public static class ScenarioAtlas
    {
        private static readonly Lazy<Dictionary<string, IScenario>> _byId;

        static ScenarioAtlas()
        {
            _byId = new Lazy<Dictionary<string, IScenario>>(() => typeof(ScenarioAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<IScenario>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, IScenario> ById => _byId.Value;

        public static IScenario NomadsTent => new SimpleScenario(
            id: "SCENARIO_NOMADS_TENT",
            firstStep: new SimpleScenarioStep(
                description: "Step description",
                actions: new List<ScenarioStepAction>
                {
                    ScenarioStepAction.End("Option description"),
                }));
    }
}

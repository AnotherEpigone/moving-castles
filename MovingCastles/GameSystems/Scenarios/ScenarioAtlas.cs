using MovingCastles.GameSystems.Levels;
using MovingCastles.Text;
using Optional;
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
                .Where(p => p.PropertyType == typeof(IScenario))
                .Select(p => p.GetValue(null))
                .OfType<IScenario>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, IScenario> ById => _byId.Value;

        public static IScenario HermitsTent => new SimpleScenario(
            id: "SCENARIO_HERMITS_TENT",
            firstStep: new SimpleScenarioStep(
                description: Story.ScenDesc_HermitTent_Empty,
                actions: new List<ScenarioStepAction>
                {
                    ScenarioStepAction.End(Story.ActDesc_HermitTent_Empty),
                }));

        public static IScenario OldAlwardsTower => new SimpleScenario(
            id: "SCENARIO_OLD_ALWARDS",
            firstStep: new SimpleScenarioStep(
                description: Story.ScenDesc_AlwardsTower,
                actions: new List<ScenarioStepAction>
                {
                    new ScenarioStepAction(
                        Story.ActDesc_AlwardsTower_Enter,
                        (dm, lm) =>
                        {
                            dm.LevelMaster.ChangeStructure(
                                Structure.StructureId_AlwardsTower,
                                LevelId.AlwardsTower1,
                                new SpawnConditions(Spawn.Default, 0),
                                dm.Player,
                                lm);
                        },
                        Option.None<IScenarioStep>()),
                    ScenarioStepAction.End(Story.ActDesc_AlwardsTower_MoveOn),
                }));
    }
}

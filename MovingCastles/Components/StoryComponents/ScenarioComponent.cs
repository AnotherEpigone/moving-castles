using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Scenarios;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Troschuetz.Random;

namespace MovingCastles.Components.StoryComponents
{
    public class ScenarioComponent : IStepTriggeredComponent, ISerializableComponent
    {
        private readonly IScenario _scenario;

        public ScenarioComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _scenario = ScenarioAtlas.ById[stateObj.ScenarioId];
        }

        public ScenarioComponent(IScenario scenario)
        {
            _scenario = scenario;
        }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster dungeonMaster, IGenerator rng)
        {
            Trigger(steppingEntity, dungeonMaster, logManager, rng);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(ScenarioComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                ScenarioId = _scenario.Id,
            }),
        };

        private void Trigger(McEntity triggeringEntity, IDungeonMaster dungeonMaster, ILogManager logManager, IGenerator rng)
        {
            if (triggeringEntity is not Wizard)
            {
                return;
            }

            dungeonMaster.ScenarioMaster.Show(_scenario, dungeonMaster, logManager, rng); // todo real scenario
        }

        [DataContract]
        private class State
        {
            [DataMember] public string ScenarioId;
        }
    }
}

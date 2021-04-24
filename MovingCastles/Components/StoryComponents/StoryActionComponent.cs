using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Scenarios;
using MovingCastles.Serialization;
using MovingCastles.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Troschuetz.Random;

namespace MovingCastles.Components.StoryComponents
{
    public class StoryActionComponent : IStepTriggeredComponent, ISerializableComponent
    {
        private readonly string _resourceKey;

        public StoryActionComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _resourceKey = stateObj.ResourceKey;
        }

        public StoryActionComponent(string resourceKey)
        {
            _resourceKey = resourceKey;
        }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster dungeonMaster, IGenerator rng)
        {
            Trigger(steppingEntity, dungeonMaster, rng);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(StoryMessageComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                ResourceKey = _resourceKey,
            }),
        };

        private void Trigger(McEntity triggeringEntity, IDungeonMaster dungeonMaster, IGenerator rng)
        {
            if (triggeringEntity is not Wizard)
            {
                return;
            }

            var story = Story.ResourceManager.GetString(_resourceKey);
            dungeonMaster.ScenarioMaster.Show(new SimpleScenario(), dungeonMaster, rng); // todo real scenario
        }

        [DataContract]
        private class State
        {
            [DataMember] public string ResourceKey;
        }
    }
}

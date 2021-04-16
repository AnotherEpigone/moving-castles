using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Serialization;
using MovingCastles.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

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

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster dungeonMaster)
        {
            Trigger(steppingEntity, dungeonMaster);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(StoryMessageComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                ResourceKey = _resourceKey,
            }),
        };

        private void Trigger(McEntity triggeringEntity, IDungeonMaster dungeonMaster)
        {
            if (triggeringEntity is not Wizard)
            {
                return;
            }

            var story = Story.ResourceManager.GetString(_resourceKey);
            dungeonMaster.StoryActionWindow.Show(true);
        }

        [DataContract]
        private class State
        {
            [DataMember] public string ResourceKey;
        }
    }
}

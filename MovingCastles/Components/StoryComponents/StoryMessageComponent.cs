using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Serialization;
using MovingCastles.Text;
using MovingCastles.Ui;
using MovingCastles.Ui.Windows;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MovingCastles.Components.StoryComponents
{
    public class StoryMessageComponent : IStepTriggeredComponent, ISerializableComponent, IInteractTriggeredComponent
    {
        private readonly string _resourceKey;
        private bool _stepTriggerActive;

        public StoryMessageComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _resourceKey = stateObj.ResourceKey;
            _stepTriggerActive = stateObj.StepTriggerActive;
        }

        public StoryMessageComponent(string resourceKey, bool stepTriggerActive)
        {
            _resourceKey = resourceKey;
            _stepTriggerActive = stepTriggerActive;
        }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster gameManager)
        {
            if (!_stepTriggerActive)
            {
                return;
            }

            Trigger(steppingEntity, logManager);
        }

        public void Interact(McEntity interactingEntity, ILogManager logManager, IDungeonMaster dungeonMaster)
            => Trigger(interactingEntity, logManager);

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(StoryMessageComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                ResourceKey = _resourceKey,
                StepTriggerActive = _stepTriggerActive,
            }),
        };

        private void Trigger(McEntity triggeringEntity, ILogManager logManager)
        {
            if (triggeringEntity is not Wizard)
            {
                return;
            }

            _stepTriggerActive = false;
            var story = Story.ResourceManager.GetString(_resourceKey);
            logManager.StoryLog(ColorHelper.GetParserString(story, ColorHelper.StoryBlue));
        }

        [DataContract]
        private class State
        {
            [DataMember] public string ResourceKey;
            [DataMember] public bool StepTriggerActive;
        }
    }
}

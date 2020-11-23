using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Serialization;
using MovingCastles.Text;
using MovingCastles.Ui.Windows;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MovingCastles.Components.StoryComponents
{
    public class StoryMessageBoxComponent : IStepTriggeredComponent, ISerializableComponent, IInteractTriggeredComponent
    {
        private readonly string _resourceKey;
        private bool _stepTriggerActive;

        public StoryMessageBoxComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _resourceKey = stateObj.ResourceKey;
            _stepTriggerActive = stateObj.StepTriggerActive;
        }

        public StoryMessageBoxComponent(string resourceKey, bool stepTriggerActive)
        {
            _resourceKey = resourceKey;
            _stepTriggerActive = stepTriggerActive;
        }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager)
        {
            if (!_stepTriggerActive)
            {
                return;
            }

            Trigger();
        }

        public void Interact(McEntity interactingEntity, ILogManager logManager) => Trigger();

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(StoryMessageBoxComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                ResourceKey = _resourceKey,
                StepTriggerActive = _stepTriggerActive,
            }),
        };

        private void Trigger()
        {
            _stepTriggerActive = false;
            var story = Story.ResourceManager.GetString(_resourceKey);
            var msgbox = new StoryMessageBox(story);
            msgbox.Show(true);
        }

        [DataContract]
        private class State
        {
            [DataMember] public string ResourceKey;
            [DataMember] public bool StepTriggerActive;
        }
    }
}

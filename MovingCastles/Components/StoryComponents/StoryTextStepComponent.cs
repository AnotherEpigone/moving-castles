﻿using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Text;
using MovingCastles.Ui.Windows;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MovingCastles.Components.StoryComponents
{
    public class StoryTextStepComponent : IStepTriggeredComponent, ISerializableComponent
    {
        private readonly string _resourceKey;
        private bool _stepTriggerActive;

        public StoryTextStepComponent(string state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state);
            _resourceKey = stateObj.ResourceKey;
            _stepTriggerActive = stateObj.StepTriggerActive;
        }

        public StoryTextStepComponent(string resourceKey, bool stepTriggerActive)
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

            _stepTriggerActive = false;
            var story = Story.ResourceManager.GetString(_resourceKey);
            var msgbox = new StoryMessageBox(story);
            msgbox.Show(true);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(StoryTextStepComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                ResourceKey = _resourceKey,
                StepTriggerActive = _stepTriggerActive,
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public string ResourceKey;
            [DataMember] public bool StepTriggerActive;
        }
    }
}

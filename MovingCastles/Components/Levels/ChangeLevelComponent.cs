﻿using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Troschuetz.Random;

namespace MovingCastles.Components.Levels
{
    public class ChangeLevelComponent : IStepTriggeredComponent, IInteractTriggeredComponent, ISerializableComponent
    {
        private readonly string _targetMapId;
        private readonly SpawnConditions _spawnConditions;

        public ChangeLevelComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _targetMapId = stateObj.TargetMapId;
            _spawnConditions = stateObj.SpawnConditions;
        }

        public ChangeLevelComponent(string targetMapId, SpawnConditions spawnConditions)
        {
            _targetMapId = targetMapId;
            _spawnConditions = spawnConditions;
        }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster dungeonMaster, IGenerator rng)
        {
            if (steppingEntity is not Wizard)
            {
                return;
            }

            dungeonMaster.LevelMaster.ChangeLevel(_targetMapId, _spawnConditions, dungeonMaster.Player, logManager);
        }

        public void Interact(McEntity interactingEntity, ILogManager logManager, IDungeonMaster dungeonMaster)
        {
            if (interactingEntity is not Wizard)
            {
                return;
            }

            dungeonMaster.LevelMaster.ChangeLevel(_targetMapId, _spawnConditions, dungeonMaster.Player, logManager);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(ChangeLevelComponent),
            State = JsonConvert.SerializeObject(new State()
            {
               TargetMapId = _targetMapId,
               SpawnConditions = _spawnConditions,
            }),
        };

        [DataContract]
        private class State
        {
            [DataMember] public string TargetMapId;
            [DataMember] public SpawnConditions SpawnConditions;
        }

    }
}

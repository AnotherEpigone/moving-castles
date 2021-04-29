using GoRogue.GameFramework;
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
    public class ChangeStructureComponent : IStepTriggeredComponent, IInteractTriggeredComponent, IBumpTriggeredComponent, ISerializableComponent
    {
        private readonly string _targetMapId;
        private readonly string _targetStructureId;
        private readonly SpawnConditions _spawnConditions;

        public ChangeStructureComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _targetMapId = stateObj.TargetMapId;
            _targetStructureId = stateObj.TargetStructureId;
            _spawnConditions = stateObj.SpawnConditions;
        }

        public ChangeStructureComponent(string targetStructureId, string targetMapId, SpawnConditions spawnConditions)
        {
            _targetMapId = targetMapId;
            _targetStructureId = targetStructureId;
            _spawnConditions = spawnConditions;
        }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster dungeonMaster, IGenerator rng)
        {
            if (steppingEntity is not Wizard)
            {
                return;
            }

            dungeonMaster.LevelMaster.ChangeStructure(_targetStructureId, _targetMapId, _spawnConditions, dungeonMaster.Player, logManager);
        }

        public void Interact(McEntity interactingEntity, ILogManager logManager, IDungeonMaster dungeonMaster)
        {
            if (interactingEntity is not Wizard)
            {
                return;
            }

            dungeonMaster.LevelMaster.ChangeStructure(_targetStructureId, _targetMapId, _spawnConditions, dungeonMaster.Player, logManager);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(ChangeStructureComponent),
            State = JsonConvert.SerializeObject(new State()
            {
                TargetMapId = _targetMapId,
                TargetStructureId = _targetStructureId,
                SpawnConditions = _spawnConditions,
            }),
        };

        public void Bump(McEntity bumpingEntity, ILogManager logManager, IDungeonMaster dungeonMaster, IGenerator rng)
        {
            if (bumpingEntity is not Wizard)
            {
                return;
            }

            dungeonMaster.LevelMaster.ChangeStructure(_targetStructureId, _targetMapId, _spawnConditions, dungeonMaster.Player, logManager);
        }

        [DataContract]
        private class State
        {
            [DataMember] public string TargetMapId;
            [DataMember] public string TargetStructureId;
            [DataMember] public SpawnConditions SpawnConditions;
        }

    }
}

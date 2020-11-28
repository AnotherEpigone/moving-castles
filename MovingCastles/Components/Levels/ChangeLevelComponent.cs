using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components.Levels
{
    public class ChangeLevelComponent : IStepTriggeredComponent, ISerializableComponent
    {
        private readonly string _targetMapId;
        private readonly SpawnConditions _spawnConditions;

        public ChangeLevelComponent(string targetMapId, SpawnConditions spawnConditions)
        {
            _targetMapId = targetMapId;
            _spawnConditions = spawnConditions;
        }

        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IDungeonMaster gameManager)
        {
            throw new System.NotImplementedException();
        }

        public ComponentSerializable GetSerializable()
        {
            throw new System.NotImplementedException();
        }
    }
}

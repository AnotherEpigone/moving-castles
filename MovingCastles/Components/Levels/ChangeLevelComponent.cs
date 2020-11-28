using GoRogue.GameFramework;
using MovingCastles.Components.Serialization;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.Components.Levels
{
    public class ChangeLevelComponent : IStepTriggeredComponent, ISerializableComponent
    {
        public IGameObject Parent { get; set; }

        public void OnStep(McEntity steppingEntity, ILogManager logManager, IGameManager gameManager)
        {
            throw new System.NotImplementedException();
        }

        public ComponentSerializable GetSerializable()
        {
            throw new System.NotImplementedException();
        }
    }
}

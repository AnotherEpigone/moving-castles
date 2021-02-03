using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems.Time;

namespace MovingCastles.Components.Effects
{
    public interface ITimedEffect : IGameObjectComponent, ISerializableComponent
    {
        void OnTick(McTimeSpan time, GameSystems.Logging.ILogManager logManager);
    }
}
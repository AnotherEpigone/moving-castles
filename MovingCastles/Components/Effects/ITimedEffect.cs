using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems.Time;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems;

namespace MovingCastles.Components.Effects
{
    public interface ITimedEffect : IGameObjectComponent, ISerializableComponent
    {
        void OnTick(McTimeSpan time, ILogManager logManager, IDungeonMaster dungeonMaster);
    }
}
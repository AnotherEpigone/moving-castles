using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems.Time;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems;
using Troschuetz.Random;

namespace MovingCastles.Components.Effects
{
    public interface ITimedEffect : ISerializableComponent
    {
        void OnTick(McTimeSpan time, ILogManager logManager, IDungeonMaster dungeonMaster, IGenerator rng);
    }
}
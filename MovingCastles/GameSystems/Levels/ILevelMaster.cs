using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using System;

namespace MovingCastles.GameSystems.Levels
{
    public interface ILevelMaster
    {
        Level Level { get; set; }

        Structure Structure { get; set; }

        event EventHandler LevelChanged;

        void ChangeLevel(string targetMapId, SpawnConditions spawnConditions, Wizard player, ILogManager logManager);
    }
}

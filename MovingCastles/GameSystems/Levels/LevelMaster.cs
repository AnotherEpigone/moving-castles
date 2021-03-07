using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using System;

namespace MovingCastles.GameSystems.Levels
{
    public class LevelMaster : ILevelMaster
    {
        public event EventHandler LevelChanged;

        public Level Level { get; set; }

        public Structure Structure { get; set; }

        public void ChangeLevel(string targetMapId, SpawnConditions spawnConditions, Wizard player, ILogManager logManager)
        {
            Level = Structure.GetLevel(targetMapId, player, spawnConditions);

            logManager.StoryLog($"Entered {Level.Name}.");
            LevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

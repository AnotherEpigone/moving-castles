using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Player;
using System;

namespace MovingCastles.GameSystems.Levels
{
    public class LevelMaster : ILevelMaster
    {
        public event EventHandler LevelChanged;

        public Level Level { get; set; }

        public Structure Structure { get; set; }

        public void ChangeLevel(string targetMapId, SpawnConditions spawnConditions, PlayerTemplate player, ILogManager logManager)
        {
            Level = Structure.GetLevel(targetMapId, player, spawnConditions);

            logManager.StoryLog($"Entered {Level.Name}.");
            LevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

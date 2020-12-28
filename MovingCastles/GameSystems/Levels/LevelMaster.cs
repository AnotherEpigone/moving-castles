using MovingCastles.GameSystems.Player;
using System;

namespace MovingCastles.GameSystems.Levels
{
    public class LevelMaster : ILevelMaster
    {
        public event EventHandler LevelChanged;

        public Level Level { get; set; }

        public Structure Structure { get; set; }

        public void ChangeLevel(string targetMapId, SpawnConditions spawnConditions, PlayerInfo player)
        {
            Level = Structure.GetLevel(targetMapId, player, spawnConditions);
            LevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

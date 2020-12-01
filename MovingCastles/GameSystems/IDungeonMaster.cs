using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;
using System;

namespace MovingCastles.GameSystems
{
    public interface IDungeonMaster
    {
        Level Level { get; set; }
        PlayerInfo Player { get; }
        Structure Structure { get; set; }

        event EventHandler LevelChanged;

        void ChangeLevel(string targetMapId, SpawnConditions spawnConditions);
    }
}
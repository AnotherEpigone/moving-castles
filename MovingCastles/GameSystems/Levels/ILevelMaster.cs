using MovingCastles.GameSystems.Player;
using System;

namespace MovingCastles.GameSystems.Levels
{
    public interface ILevelMaster
    {
        Level Level { get; set; }

        Structure Structure { get; set; }

        event EventHandler LevelChanged;

        void ChangeLevel(string targetMapId, SpawnConditions spawnConditions, PlayerInfo player);
    }
}

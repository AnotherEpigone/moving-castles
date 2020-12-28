using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;
using System;

namespace MovingCastles.GameSystems
{
    public class DungeonMaster : IDungeonMaster
    {
        public DungeonMaster(
            PlayerInfo player,
            ILevelMaster levelMaster)
        {
            Player = player;
            LevelMaster = levelMaster;
        }

        public PlayerInfo Player { get; }

        public ILevelMaster LevelMaster { get; }
    }
}

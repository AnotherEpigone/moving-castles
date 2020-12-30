using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;
using System;

namespace MovingCastles.GameSystems
{
    public interface IDungeonMaster
    {
        PlayerInfo Player { get; }

        ILevelMaster LevelMaster { get; }

        public IFactionMaster FactionMaster { get; }
    }
}
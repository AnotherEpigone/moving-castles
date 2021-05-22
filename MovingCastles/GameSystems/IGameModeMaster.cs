using MovingCastles.Entities;
using MovingCastles.Ui.Consoles;
using SadConsole;
using System;

namespace MovingCastles.GameSystems
{
    public enum GameMode
    {
        Castle,
        Dungeon,
    }

    public interface IGameModeMaster
    {
        event EventHandler ModeChanged;

        GameMode Mode { get; }

        Font Font { get; }

        IEntityFactory EntityFactory { get; }

        ITurnBasedGameConsoleFactory GameConsoleFactory { get; }

        void SetGameMode(GameMode gameMode, Action levelLoadAction);
    }
}

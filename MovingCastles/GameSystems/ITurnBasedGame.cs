using MovingCastles.Entities;
using MovingCastles.Maps;

namespace MovingCastles.GameSystems
{
    public interface ITurnBasedGame
    {
        MovingCastlesMap Map { get; set; }

        bool HandleAsPlayerInput(SadConsole.Input.Keyboard info);
        void RegisterEntity(McEntity entity);
        void UnregisterEntity(McEntity entity);
        void RegisterPlayer(Player player);
    }
}
using MovingCastles.Entities;
using MovingCastles.Maps;

namespace MovingCastles.GameSystems.TurnBasedGame
{
    public interface ITurnBasedGame
    {
        DungeonMap Map { get; set; }
        State State { get; set; }

        bool HandleAsPlayerInput(SadConsole.Input.Keyboard info);
        void RegisterEntity(McEntity entity);
        void UnregisterEntity(McEntity entity);
        void RegisterPlayer(Wizard player);
    }
}
namespace MovingCastles.GameSystems
{
    public interface ITurnBasedGame
    {
        bool HandleAsPlayerInput(SadConsole.Input.Keyboard info);
    }
}
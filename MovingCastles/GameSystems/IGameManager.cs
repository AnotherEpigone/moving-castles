namespace MovingCastles.GameSystems
{
    public interface IGameManager
    {
        void StartDungeonModeDemo();

        void StartCastleModeDemo();

        void StartMapGenDemo();

        void StartNewGame();

        void Save();

        void Load();

        bool CanLoad();
    }
}
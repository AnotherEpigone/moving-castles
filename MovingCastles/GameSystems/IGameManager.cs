namespace MovingCastles.GameSystems
{
    public interface IGameManager
    {
        void StartCastleModeDemo();

        void StartMapGenDemo();

        void StartNewGame();

        void Save();

        void Load();

        bool CanLoad();
    }
}
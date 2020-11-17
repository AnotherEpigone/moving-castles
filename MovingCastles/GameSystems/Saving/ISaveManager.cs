namespace MovingCastles.GameSystems.Saving
{
    public interface ISaveManager
    {
        (bool, Save) Read();

        void Write(Save save);

        bool SaveExists();
    }
}
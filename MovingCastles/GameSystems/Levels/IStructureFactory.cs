namespace MovingCastles.GameSystems.Levels
{
    public interface IStructureFactory
    {
        Structure CreateById(string id, IGameModeMaster gameModeMaster);
    }
}
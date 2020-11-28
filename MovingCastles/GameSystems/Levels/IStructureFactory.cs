using MovingCastles.Entities;

namespace MovingCastles.GameSystems.Levels
{
    public interface IStructureFactory
    {
        Structure CreateById(string id, IEntityFactory entityFactory);
    }
}
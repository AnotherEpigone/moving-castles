using SadConsole;

namespace MovingCastles.Maps
{
    public interface IMapFactory
    {
        DungeonMap CreateDungeonMap(int width, int height, IMapPlan mapPlan);
        CastleMap CreateCastleMap(int width, int height, IMapPlan mapPlan);
    }
}
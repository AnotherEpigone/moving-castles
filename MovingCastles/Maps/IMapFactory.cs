using MovingCastles.Entities;

namespace MovingCastles.Maps
{
    public interface IMapFactory
    {
        McMap CreateCastleMap(int width, int height, MapTemplate mapPlan, Wizard player);
    }
}
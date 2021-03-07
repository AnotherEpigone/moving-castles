using MovingCastles.Entities;

namespace MovingCastles.Maps
{
    public interface IMapFactory
    {
        CastleMap CreateCastleMap(int width, int height, MapTemplate mapPlan, Wizard player);
    }
}
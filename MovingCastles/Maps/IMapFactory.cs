using MovingCastles.GameSystems.Player;

namespace MovingCastles.Maps
{
    public interface IMapFactory
    {
        DungeonMap CreateDungeonMap(int width, int height, MapTemplate mapPlan, PlayerTemplate playerInfo);
        CastleMap CreateCastleMap(int width, int height, MapTemplate mapPlan, PlayerTemplate playerInfo);
    }
}
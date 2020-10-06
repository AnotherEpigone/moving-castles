using MovingCastles.GameSystems.PlayerInfo;

namespace MovingCastles.Maps
{
    public interface IMapFactory
    {
        DungeonMap CreateDungeonMap(int width, int height, IMapPlan mapPlan, PlayerInfo playerInfo);
        CastleMap CreateCastleMap(int width, int height, IMapPlan mapPlan, PlayerInfo playerInfo);
    }
}
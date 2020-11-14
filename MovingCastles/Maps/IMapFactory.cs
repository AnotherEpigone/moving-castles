using MovingCastles.GameSystems.Player;

namespace MovingCastles.Maps
{
    public interface IMapFactory
    {
        DungeonMap CreateDungeonMap(int width, int height, MapTemplate mapPlan, PlayerInfo playerInfo);
        CastleMap CreateCastleMap(int width, int height, MapTemplate mapPlan, PlayerInfo playerInfo);
        DungeonMap CreateMapGenTestAreaMap(int width, int height, MapTemplate mapPlan, PlayerInfo playerInfo);
    }
}
using GoRogue;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.PlayerInfo;

namespace MovingCastles.Entities
{
    public interface IEntityFactory
    {
        McEntity CreateActor(int glyph, Coord position, string name);
        McEntity CreateItem(Coord position, ItemTemplate itemTemplate);
        Player CreatePlayer(Coord position, PlayerInfo playerInfo);
        Castle CreateCastle(Coord position, PlayerInfo playerInfo);
    }
}
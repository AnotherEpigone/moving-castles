using GoRogue;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Player;

namespace MovingCastles.Entities
{
    public interface IEntityFactory
    {
        McEntity CreateActor(int glyph, Coord position, string name);
        McEntity CreateItem(Coord position, ItemTemplate itemTemplate);
        Wizard CreatePlayer(Coord position, Player playerInfo);
        Castle CreateCastle(Coord position, Player playerInfo);
    }
}
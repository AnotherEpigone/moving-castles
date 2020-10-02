using GoRogue;
using MovingCastles.GameSystems.Items;

namespace MovingCastles.Entities
{
    public interface IEntityFactory
    {
        McEntity CreateActor(int glyph, Coord position, string name);
        McEntity CreateItem(Coord position, ItemTemplate itemTemplate);
        Player CreatePlayer(Coord position);
        Castle CreateCastle(Coord position);
    }
}
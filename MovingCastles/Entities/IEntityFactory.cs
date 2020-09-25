using GoRogue;
using SadConsole;

namespace MovingCastles.Entities
{
    public interface IEntityFactory
    {
        McEntity CreateActor(int glyph, Coord position, string name);

        McEntity CreateItem(int glyph, Coord position, string name, string desc);
        Player CreatePlayer(Coord position);
    }
}
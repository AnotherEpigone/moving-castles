using GoRogue;
using SadConsole;

namespace MovingCastles.Entities
{
    public interface IEntityFactory
    {
        BasicEntity CreateActor(int glyph, Coord position, string name);

        BasicEntity CreateItem(int glyph, Coord position, string name, string desc);
    }
}
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace MovingCastles.Entities
{
    /// <summary>
    /// It stands for Moving Castles entity... pun definitely intended.
    /// </summary>
    public class McEntity : BasicEntity
    {
        public McEntity(
            string name,
            Color foreground,
            Color background,
            int glyph,
            Coord position,
            int layer,
            bool isWalkable,
            bool isTransparent)
            : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            Name = name;
        }
    }
}

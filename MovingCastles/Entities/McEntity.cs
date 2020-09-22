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

        public event System.EventHandler<ItemMovedEventArgs<McEntity>> Bumped;

        public event System.EventHandler RemovedFromMap;

        public void Move(Direction direction)
        {
            if (CurrentMap.WalkabilityView[Position + direction])
            {
                Position += direction;
            }
            else
            {
                // can't move because we just bumped into something solid
                Bumped?.Invoke(this, new ItemMovedEventArgs<McEntity>(this, Position, Position + direction));
            }
        }

        public void Remove()
        {
            CurrentMap.RemoveEntity(this);
            RemovedFromMap?.Invoke(this, System.EventArgs.Empty);
        }
    }
}

using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Serialization.Entities;
using MovingCastles.Ui;
using Newtonsoft.Json;
using SadConsole;
using System.Diagnostics;

namespace MovingCastles.Entities
{
    /// <summary>
    /// It stands for Moving Castles entity... pun definitely intended.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(McEntityJsonConverter))]
    public class McEntity : BasicEntity
    {
        public McEntity(
            string templateId,
            string name,
            Color foreground,
            Color background,
            int glyph,
            Coord position,
            int layer,
            bool isWalkable,
            bool isTransparent,
            Color nameColor)
            : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            TemplateId = templateId;
            Name = name;
            NameColor = nameColor;
        }

        public event System.EventHandler<ItemMovedEventArgs<McEntity>> Bumped;

        public event System.EventHandler RemovedFromMap;

        public bool HasMap => CurrentMap != null;

        public Color NameColor { get; }

        public string TemplateId { get; }

        public string ColoredName => ColorHelper.GetParserString(Name, NameColor);

        public virtual string GetFlavorDescription()
        {
            if (!HasMap)
            {
                return string.Empty;
            }

            if (Layer == (int)Maps.DungeonMapLayer.GHOSTS)
            {
                return $"You remember seeing a {ColoredName} here.";
            }

            return $"You see a {ColoredName}.";
        }

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

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(McEntity)}: {Name}");
            }
        }
    }
}

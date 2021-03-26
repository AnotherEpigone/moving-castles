using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.GameSystems.Movement;
using MovingCastles.Serialization.Entities;
using MovingCastles.Ui;
using Newtonsoft.Json;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MovingCastles.Entities
{
    public enum MoveOutcome
    {
        Move,
        NoMove,
        Melee,
    }

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
            Color nameColor,
            string faction,
            Guid id)
            : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            TemplateId = templateId;
            Name = name;
            NameColor = nameColor;
            FactionName = faction;
            UniqueId = id;

            SubTiles = new List<McEntity>();
        }

        public event EventHandler<EntityBumpedEventArgs> Bumped;
        public event EventHandler RemovedFromMap;

        public string FactionName { get; }

        public bool HasMap => CurrentMap != null;

        public Guid UniqueId { get; }

        public Color NameColor { get; }

        public string TemplateId { get; }

        /// <summary>
        /// Null for standalone entities. If this is part of a multi-tile entity, Anchor is
        /// the entity which can be interacted with.
        /// </summary>
        public McEntity Anchor { get; init; }

        /// <summary>
        /// Add child entities to make this the anchor of a multi-tile entity
        /// </summary>
        public IList<McEntity> SubTiles { get; }

        public bool IsSubTile => Anchor != null;

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

        public MoveOutcome Move(Direction direction)
        {
            if (CurrentMap.WalkabilityView[Position + direction])
            {
                Position += direction;
                return MoveOutcome.Move;
            }
            else
            {
                // can't move because we just bumped into something solid
                var bumpedEventArgs = new EntityBumpedEventArgs(this, Position + direction);
                Bumped?.Invoke(this, bumpedEventArgs);

                return bumpedEventArgs.MadeMeleeAttack ? MoveOutcome.Melee : MoveOutcome.NoMove;
            }
        }

        public void Remove()
        {
            CurrentMap.RemoveEntity(this);
            RemovedFromMap?.Invoke(this, EventArgs.Empty);
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

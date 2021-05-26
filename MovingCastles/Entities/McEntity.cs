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
using System.Linq;

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
            Glyph = glyph;

            SubTiles = new List<McEntity>();
        }

        public event EventHandler<EntityBumpedEventArgs> Bumped;
        public event EventHandler RemovedFromMap;
        public event EventHandler ComponentsChanged;

        public string FactionName { get; }

        public bool HasMap => CurrentMap != null;

        public Guid UniqueId { get; }

        public Color NameColor { get; }

        public string TemplateId { get; }

        public int Glyph { get; }

        /// <summary>
        /// Null for standalone entities. If this is part of a multi-tile entity, Anchor is
        /// the entity which can be interacted with.
        /// </summary>
        public McEntity Anchor { get; set; }

        /// <summary>
        /// Add child entities to make this the anchor of a multi-tile entity
        /// </summary>
        public IList<McEntity> SubTiles { get; }

        public bool IsSubTile => Anchor != null;

        public string ColoredName => ColorHelper.GetParserString(Name, NameColor);

        public virtual string GetFlavorDescription()
        {
            if (IsSubTile)
            {
                return Anchor.GetFlavorDescription();
            }

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
            var tiles = SubTiles
                .Select(st => st.Position)
                .Append(Position);
            var bumpedEvents = new List<EntityBumpedEventArgs>();
            foreach (var tile in tiles)
            {
                var target = tile + direction;
                if (tiles.Contains(target))
                {
                    continue;
                }

                if (!CurrentMap.WalkabilityView[target])
                {
                    var bumpedEventArgs = new EntityBumpedEventArgs(this, target);
                    Bumped?.Invoke(this, bumpedEventArgs);

                    bumpedEvents.Add(bumpedEventArgs);
                }
            }

            if (bumpedEvents.Count == 0)
            {
                BulkMove(SubTiles.Append(this), direction);
                return MoveOutcome.Move;
            }

            return bumpedEvents.Any(be => be.MadeMeleeAttack) ? MoveOutcome.Melee : MoveOutcome.NoMove;
        }

        public void Remove()
        {
            foreach (var subTile in SubTiles)
            {
                subTile.Remove();
            }

            CurrentMap.RemoveEntity(this);
            RemovedFromMap?.Invoke(this, EventArgs.Empty);
        }

        public new void AddGoRogueComponent(object component)
        {
            if (IsSubTile)
            {
                Anchor.AddGoRogueComponent(component);
            }
            else
            {
                base.AddGoRogueComponent(component);
                ComponentsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public new void RemoveGoRogueComponent(object component)
        {
            if (IsSubTile)
            {
                Anchor.RemoveGoRogueComponent(component);
            }
            else
            {
                base.RemoveGoRogueComponent(component);
                ComponentsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public new void RemoveGoRogueComponents(params object[] components)
        {
            if (IsSubTile)
            {
                Anchor.RemoveGoRogueComponents(components);
            }
            else
            {
                base.RemoveGoRogueComponents(components);
                ComponentsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public new T GetGoRogueComponent<T>()
        {
            return IsSubTile
                ? Anchor.GetGoRogueComponent<T>()
                : base.GetGoRogueComponent<T>();
        }

        public new IEnumerable<T> GetGoRogueComponents<T>()
        {
            return IsSubTile
                ? Anchor.GetGoRogueComponents<T>()
                : base.GetGoRogueComponents<T>();
        }

        public new bool HasGoRogueComponent(Type componentType)
        {
            return IsSubTile
                ? Anchor.HasGoRogueComponent(componentType)
                : base.HasGoRogueComponent(componentType);
        }

        public new bool HasGoRogueComponent<T>()
        {
            return IsSubTile
                ? Anchor.HasGoRogueComponent<T>()
                : base.HasGoRogueComponent<T>();
        }

        public new bool HasGoRogueComponents(params Type[] componentTypes)
        {
            return IsSubTile
                ? Anchor.HasGoRogueComponents(componentTypes)
                : base.HasGoRogueComponents(componentTypes);
        }

        private static void BulkMove(IEnumerable<McEntity> entities, Direction direction)
        {
            const int maxCycles = 100;
            var cycles = 0;
            var toMove = new List<McEntity>(entities);
            while (toMove.Count > 0 && cycles < maxCycles)
            {
                var moved = new List<McEntity>();
                foreach (var entity in toMove)
                {
                    var oldPos = entity.Position;
                    entity.Position += direction;
                    if (entity.Position != oldPos)
                    {
                        moved.Add(entity);
                    }
                }

                foreach (var movedEntity in moved)
                {
                    toMove.Remove(movedEntity);
                }

                cycles++;
            }
        }

        private string DebuggerDisplay
        {
            get
            {
                return IsSubTile
                    ? string.Format($"{nameof(McEntity)}: {Name} Subtile")
                    : string.Format($"{nameof(McEntity)}: {Name}");
            }
        }
    }
}

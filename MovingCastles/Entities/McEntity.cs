﻿using GoRogue;
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
                    var bumpedEventArgs = new EntityBumpedEventArgs(this, Position + direction);
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
            CurrentMap.RemoveEntity(this);
            RemovedFromMap?.Invoke(this, EventArgs.Empty);
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
                return string.Format($"{nameof(McEntity)}: {Name}");
            }
        }
    }
}

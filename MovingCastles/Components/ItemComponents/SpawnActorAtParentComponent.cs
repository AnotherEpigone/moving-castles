﻿using GoRogue;
using GoRogue.GameFramework;
using MovingCastles.Components.Effects;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.Extensions;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Time;
using MovingCastles.Maps;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Serialization;
using Troschuetz.Random;

namespace MovingCastles.Components.ItemComponents
{
    /// <summary>
    /// Spawn an actor from a template next to the parent entity after a set number of ticks
    /// </summary>
    public class SpawnActorAtParentComponent : ITimedEffect
    {
        private readonly string _templateId;

        private int _ticks;
        private long _startTime;
        private int _elapsed;
        private IGameObject _parent;

        public SpawnActorAtParentComponent(SerializedObject state)
        {
            var stateObj = JsonConvert.DeserializeObject<State>(state.Value);
            _templateId = stateObj.TemplateId;
            _ticks = stateObj.Ticks;
            _startTime = stateObj.StartTime;
        }

        public SpawnActorAtParentComponent(string templateId, int ticks)
        {
            _templateId = templateId;
            _ticks = ticks;
            _elapsed = 0;
            _startTime = long.MinValue;
        }

        public IGameObject Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                if (_parent == null)
                {
                    _ticks -= _elapsed;
                    _startTime = long.MinValue;
                }
            }
        }

        public void OnTick(McTimeSpan time, ILogManager logManager, IDungeonMaster dungeonMaster, IGenerator rng)
        {
            if (!(Parent is McEntity mcParent))
            {
                return;
            }

            if (_startTime == long.MinValue)
            {
                _startTime = time.Ticks;
                return;
            }

            _elapsed = (int)(time.Ticks - _startTime);
            if (_elapsed < _ticks)
            {
                return;
            }

            var parentPos = Parent.Position;
            Parent.RemoveComponent(this);

            var mapConsole = dungeonMaster.GetCurrentMapConsole().ValueOr(() => null);
            var map = mapConsole?.Map;
            if (map == null)
            {
                return;
            }

            var actorTemplate = ActorAtlas.ActorsById[_templateId];
            var actorPlacementView = MapViewHelper.MultiTileWalkableEmptyLayerView(
                map,
                DungeonMapLayer.ACTORS,
                actorTemplate.SubTiles.Select(st => (Coord)st.Offset));

            Coord spawnPosition = Coord.NONE;
            foreach (var direction in AdjacencyRule.EIGHT_WAY.DirectionsOfNeighbors().Randomize(rng))
            {
                var pos = parentPos + direction;

                // attempt to find a spot for multi-tile entities by pushing them back if they
                // overlap a non-walkable spawner
                while (!mcParent.IsWalkable && actorTemplate.SubTiles.Any(st => pos + st.Offset == parentPos))
                {
                    pos += direction;
                }

                if (actorPlacementView[pos])
                {
                    spawnPosition = pos;
                    break;
                }
            }

            if (spawnPosition == Coord.NONE)
            {
                return;
            }

            var actor = dungeonMaster.ModeMaster.EntityFactory.CreateActor(spawnPosition, actorTemplate);
            mapConsole.AddEntity(actor);

            logManager.StoryLog($"{actor.ColoredName} spawned from {mcParent.ColoredName}.");
        }

        public ComponentSerializable GetSerializable()
        {
            return new ComponentSerializable()
            {
                Id = nameof(SpawnActorAtParentComponent),
                State = JsonConvert.SerializeObject(new State
                    {
                        Ticks = _ticks,
                        TemplateId = _templateId,
                        StartTime = _startTime,
                    }),
            };
        }

        [DataContract]
        private class State
        {
            [DataMember] public int Ticks;
            [DataMember] public long StartTime;
            [DataMember] public string TemplateId;
        }
    }
}

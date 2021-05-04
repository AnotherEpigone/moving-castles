using GoRogue;
using GoRogue.GameFramework;
using MovingCastles.Components.Effects;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Time;
using MovingCastles.Maps;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Components.ItemComponents
{
    /// <summary>
    /// Spawn an actor from a template next to the parent entity after a set number of ticks
    /// </summary>
    public class SpawnActorAtParentComponent : ITimedEffect
    {
        private readonly string _templateId;
        private readonly int _ticks;

        private long _startTime;

        public SpawnActorAtParentComponent(string templateId, int ticks)
        {
            _templateId = templateId;
            _ticks = ticks;
            _startTime = long.MinValue;
        }

        public IGameObject Parent { get; set; }

        public void OnTick(McTimeSpan time, ILogManager logManager, IDungeonMaster dungeonMaster)
        {
            if (_startTime == long.MinValue)
            {
                _startTime = time.Ticks;
                return;
            }

            var elapsed = time.Ticks - _startTime;
            if (elapsed < _ticks)
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
            foreach (var pos in AdjacencyRule.EIGHT_WAY.Neighbors(parentPos))
            {
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

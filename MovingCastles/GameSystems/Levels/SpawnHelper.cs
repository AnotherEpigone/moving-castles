using GoRogue;
using GoRogue.MapViews;
using MovingCastles.Entities;
using MovingCastles.Extensions;
using MovingCastles.Maps;
using System;
using System.Linq;
using Troschuetz.Random;

namespace MovingCastles.GameSystems.Levels
{
    public static class SpawnHelper
    {
        public static Coord GetSpawnPosition(Level level, SpawnConditions conditions, IGenerator rng)
        {
            return conditions.Spawn switch
            {
                Spawn.NewGame => level.Map.WalkabilityView.RandomPosition(true, rng),
                Spawn.Default => level.Map.WalkabilityView.RandomPosition(true, rng),
                Spawn.Stairdown => GetEntityWithTemplateId(level, DungeonModeDoodadAtlas.StaircaseDown.Id, conditions.LandmarkId)
                                        .Position,
                Spawn.StairUp => GetEntityWithTemplateId(level, DungeonModeDoodadAtlas.StaircaseUp.Id, conditions.LandmarkId)
                                        .Position,
                Spawn.Door => throw new NotSupportedException(conditions.Spawn.ToString()),
                _ => throw new ArgumentException(conditions.Spawn.ToString()),
            };
        }

        public static Coord GetAdjacentSpawnPosition(Level level, DungeonMapLayer layer, Coord anchorPosition, IGenerator rng)
        {
            var spawnView = MapViewHelper.WalkableEmptyLayerView(level.Map, layer);
            foreach (var position in AdjacencyRule.EIGHT_WAY.Neighbors(anchorPosition).Randomize(rng))
            {
                if (spawnView[position])
                {
                    return position;
                }
            }

            return Coord.NONE;
        }

        private static McEntity GetEntityWithTemplateId(Level level, string id, int index)
        {
            var doodads = level.Map.Entities.Items
                        .OfType<McEntity>()
                        .Where(e => e.TemplateId == id)
                        .ToList();
            return doodads[index];
        }
    }
}

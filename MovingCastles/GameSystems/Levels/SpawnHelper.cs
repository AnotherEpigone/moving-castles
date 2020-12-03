using GoRogue;
using GoRogue.MapViews;
using MovingCastles.Entities;
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
                Spawn.Default => level.Map.WalkabilityView.RandomPosition(true, rng),
                Spawn.Stairdown => GetEntityWithTemplateId(level, DoodadAtlas.StaircaseDown.Id, conditions.LandmarkId)
                                        .Position,
                Spawn.StairUp => GetEntityWithTemplateId(level, DoodadAtlas.StaircaseUp.Id, conditions.LandmarkId)
                                        .Position,
                Spawn.Door => throw new NotSupportedException(conditions.Spawn.ToString()),
                _ => throw new ArgumentException(conditions.Spawn.ToString()),
            };
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

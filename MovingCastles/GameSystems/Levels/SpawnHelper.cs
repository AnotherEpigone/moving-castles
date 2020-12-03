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
            switch (conditions.Spawn)
            {
                case Spawn.Default:
                    return level.Map.WalkabilityView.RandomPosition(true, rng);
                case Spawn.Stairdown:
                    return GetEntityWithTemplateId(level, DoodadAtlas.StaircaseDown.Id, conditions.LandmarkId)
                        .Position;
                case Spawn.StairUp:
                    return GetEntityWithTemplateId(level, DoodadAtlas.StaircaseUp.Id, conditions.LandmarkId)
                        .Position;
                case Spawn.Door:
                    throw new NotSupportedException(conditions.Spawn.ToString());
                default:
                    throw new ArgumentException(conditions.Spawn.ToString());
            }
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

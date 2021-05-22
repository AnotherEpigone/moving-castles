using GoRogue.MapViews;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Saving;
using MovingCastles.Maps;
using MovingCastles.Serialization.Map;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public abstract class LevelGenerator : ILevelGenerator
    {
        protected LevelGenerator(IGameModeMaster gameModeMaster)
        {
            GameModeMaster = gameModeMaster;
        }

        public abstract string Id { get; }

        protected IGameModeMaster GameModeMaster { get; }

        public abstract Level Generate(int seed, string id, Wizard player, SpawnConditions playerSpawnConditions);

        public Level Generate(Save save)
        {
            var rng = new StandardGenerator(save.MapState.Seed);
            var level = RestoreTerrainAndEntities(save.MapState, rng);

            level.Map.AddEntity(save.Wizard);

            return level;
        }

        public Level Generate(MapState mapState, Wizard player, SpawnConditions playerSpawnConditions)
        {
            var rng = new StandardGenerator(mapState.Seed);
            var level = RestoreTerrainAndEntities(mapState, rng);

            // spawn player
            player.Position = SpawnHelper.GetSpawnPosition(level, playerSpawnConditions, rng);
            level.Map.AddEntity(player);

            return level;
        }

        protected abstract (Level, LevelGenerationMetadata) GenerateTerrain(IGenerator rng, int seed, string id, int width, int height);

        private Level RestoreTerrainAndEntities(MapState mapState, IGenerator rng)
        {
            var (level, _) = GenerateTerrain(
                rng,
                mapState.Seed,
                mapState.Id,
                mapState.Width,
                mapState.Height);

            // restore terrain before entities
            level.Map.Explored = new ArrayMap<bool>(mapState.Explored, mapState.Width);
            level.Map.FovVisibilityHandler.RefreshExploredTerrain();

            foreach (var entity in mapState.Entities)
            {
                // TODO remove this terrain hack after terrain serialization is fixed
                level.Map.SetTerrain(TerrainSpawning.SpawnDungeonTerrain(entity.Position, true));
                level.Map.AddEntity(entity);
            }

            foreach (var door in mapState.Doors)
            {
                // TODO remove this terrain hack after terrain serialization is fixed
                level.Map.SetTerrain(TerrainSpawning.SpawnDungeonTerrain(door.Position, true));
                level.Map.AddEntity(door);
            }

            return level;
        }
    }
}

using GoRogue.MapViews;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Saving;
using MovingCastles.Serialization.Map;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace MovingCastles.GameSystems.Levels.Generators
{
    public abstract class LevelGenerator : ILevelGenerator
    {
        protected LevelGenerator(IEntityFactory entityFactory)
        {
            EntityFactory = entityFactory;
        }

        public abstract string Id { get; }

        protected IEntityFactory EntityFactory { get; }

        public abstract Level Generate(int seed, string id, PlayerTemplate playerInfo, SpawnConditions playerSpawnConditions);

        public Level Generate(Save save)
        {
            var rng = new StandardGenerator(save.MapState.Seed);
            var level = RestoreTerrainAndEntities(save.MapState, rng);

            level.Map.AddEntity(save.Wizard);

            return level;
        }

        public Level Generate(MapState mapState, PlayerTemplate playerInfo, SpawnConditions playerSpawnConditions)
        {
            var rng = new StandardGenerator(mapState.Seed);
            var level = RestoreTerrainAndEntities(mapState, rng);

            // spawn player
            var spawnPosition = SpawnHelper.GetSpawnPosition(level, playerSpawnConditions, rng);
            var player = EntityFactory.CreatePlayer(spawnPosition, playerInfo);
            level.Map.AddEntity(player);

            return level;
        }

        protected abstract Level GenerateTerrain(IGenerator rng, int seed, string id, int width, int height);

        private Level RestoreTerrainAndEntities(MapState mapState, IGenerator rng)
        {
            var level = GenerateTerrain(
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
                level.Map.AddEntity(entity);
            }

            foreach (var door in mapState.Doors)
            {
                level.Map.AddEntity(door);
            }

            return level;
        }
    }
}

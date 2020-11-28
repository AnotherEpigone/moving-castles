using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Player;

namespace MovingCastles.GameSystems
{
    public class DungeonMaster : IDungeonMaster
    {
        private readonly IStructureFactory _structureFactory;
        private readonly IEntityFactory _entityFactory;

        public DungeonMaster(
            PlayerInfo player,
            IStructureFactory structureFactory,
            IEntityFactory entityFactory)
        {
            Player = player;
            _structureFactory = structureFactory;
            _entityFactory = entityFactory;
        }

        public PlayerInfo Player { get; }

        public Level Level { get; set; }

        public Structure Structure { get; set; }

        public void ChangeLevel(string targetMapId, SpawnConditions spawnConditions)
        { }
    }
}

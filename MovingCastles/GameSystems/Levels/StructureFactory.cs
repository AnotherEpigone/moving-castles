using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels.Generators;
using System;

namespace MovingCastles.GameSystems.Levels
{
    public class StructureFactory : IStructureFactory
    {
        public Structure CreateById(string id, IGameModeMaster gameModeMaster)
        {
            return id switch
            {
                Structure.StructureId_MapgenDemo => CreateMapgenTest(id, gameModeMaster),
                Structure.StructureId_AlwardsTower => CreateAlwardsTower(id, gameModeMaster),
                Structure.StructureId_SaraniDesert_Highlands => CreateSaraniHighlands(id, gameModeMaster),
                _ => throw new ArgumentException($"Unknown id: {id}"),
            };
        }

        private static Structure CreateAlwardsTower(string id, IGameModeMaster gameModeMaster)
        {
            var structure = new Structure(id, GameMode.Dungeon);
            structure.Generators.Add(
                LevelId.AlwardsTower1,
                new AlwardsTowerLevelGenerator(gameModeMaster));
            structure.Generators.Add(
                LevelId.AlwardsTower2,
                new AlwardsTowerLevelGenerator(gameModeMaster));

            return structure;
        }

        private static Structure CreateSaraniHighlands(string id, IGameModeMaster gameModeMaster)
        {
            var structure = new Structure(id, GameMode.Castle);
            structure.Generators.Add(
                LevelId.SaraniHighlands,
                new SaraniHighlandsLevelGenerator(gameModeMaster));

            return structure;
        }

        private static Structure CreateMapgenTest(string id, IGameModeMaster gameModeMaster)
        {
            var structure = new Structure(id, GameMode.Dungeon);
            structure.Generators.Add(
                LevelId.MapgenTest,
                new MapgenDemoLevelGenerator(gameModeMaster));

            return structure;
        }
    }
}

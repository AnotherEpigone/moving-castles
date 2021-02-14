using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels.Generators;
using System;

namespace MovingCastles.GameSystems.Levels
{
    public class StructureFactory : IStructureFactory
    {
        public Structure CreateById(string id, IEntityFactory entityFactory)
        {
            return id switch
            {
                Structure.StructureId_MapgenDemo => CreateMapgenTest(id, entityFactory),
                Structure.StructureId_AlwardsTower => CreateAlwardsTower(id, entityFactory),
                _ => throw new ArgumentException($"Unknown id: {id}"),
            };
        }

        private static Structure CreateAlwardsTower(string id, IEntityFactory entityFactory)
        {
            var structure = new Structure(id);
            structure.Generators.Add(
                LevelId.AlwardsTower1,
                new AlwardsTowerLevelGenerator(entityFactory));
            structure.Generators.Add(
                LevelId.AlwardsTower2,
                new AlwardsTowerLevelGenerator(entityFactory));

            return structure;
        }

        private static Structure CreateMapgenTest(string id, IEntityFactory entityFactory)
        {
            var structure = new Structure(id);
            structure.Generators.Add(
                LevelId.MapgenTest,
                new MapgenDemoLevelGenerator(entityFactory));

            return structure;
        }
    }
}

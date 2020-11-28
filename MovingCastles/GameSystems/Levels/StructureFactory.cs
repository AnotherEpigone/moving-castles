using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels.Generators;
using System;

namespace MovingCastles.GameSystems.Levels
{
    public class StructureFactory : IStructureFactory
    {
        public Structure CreateById(string id, IEntityFactory entityFactory)
        {
            switch (id)
            {
                case Structure.StructureId_AlwardsTower:
                    return CreateAlwardsTower(id, entityFactory);
                default:
                    throw new ArgumentException($"Unknown id: {id}");
            }
        }

        private Structure CreateAlwardsTower(string id, IEntityFactory entityFactory)
        {
            var structure = new Structure(id)
            {
                Id = Structure.StructureId_AlwardsTower,
            };
            structure.Generators.Add(
                LevelId.AlwardsTower1,
                new AlwardsTowerLevelGenerator(entityFactory));
            structure.Generators.Add(
                LevelId.AlwardsTower2,
                new AlwardsTowerLevelGenerator(entityFactory));

            return structure;
        }
    }
}

using MovingCastles.Entities;
using MovingCastles.GameSystems.Levels;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Serialization.Map
{
    /// <summary>
    /// All the information needed to recreate a DungeonMap
    /// </summary>
    [DataContract]
    public class MapState
    {
        [DataMember] public string Id;
        [DataMember] public int Seed;
        [DataMember] public int Height;
        [DataMember] public int Width;
        [DataMember] public bool[] Explored;
        [DataMember] public List<McEntity> Entities;
        [DataMember] public List<Door> Doors;
        [DataMember] public string StructureId;

        // Keep for serialization
        public MapState()
        { }

        public MapState(Structure structure, Level level)
        {
            var entities = level.Map.Entities.Items.OfType<McEntity>().ToList();
            var wizard = entities.OfType<Wizard>().SingleOrDefault();
            entities.Remove(wizard);
            var doors = entities.OfType<Door>().ToList();
            foreach (var door in doors)
            {
                entities.Remove(door);
            }

            Id = level.Id;
            Seed = level.Seed;
            Width = level.Map.Width;
            Height = level.Map.Height;
            Explored = level.Map.Explored;
            Entities = entities;
            Doors = doors;
            StructureId = structure.Id;
        }
    }
}

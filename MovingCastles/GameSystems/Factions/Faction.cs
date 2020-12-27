using System.Collections.Generic;

namespace MovingCastles.GameSystems.Factions
{
    public class Faction
    {
        private const int EnemyThreshold = -100;

        public Faction(string name)
            : this(name, new Dictionary<string, int>()) { }

        public Faction(string name, Dictionary<string, int> relationships)
        {
            Name = name;
            Relationships = relationships;
        }

        public string Name { get; }

        private Dictionary<string, int> Relationships { get; }

        public bool IsEnemy(Faction other)
        {
            if (!Relationships.TryGetValue(other.Name, out var otherRelationship))
            {
                return false;
            }

            return otherRelationship < EnemyThreshold;
        }
    }
}

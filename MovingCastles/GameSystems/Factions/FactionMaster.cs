using System.Collections.Generic;

namespace MovingCastles.GameSystems.Factions
{
    public class FactionMaster : IFactionMaster
    {
        private const int EnemyThreshold = -100;
        private readonly Dictionary<FactionPair, int> _relationships;

        public FactionMaster()
        {
            _relationships = Faction.DefaultRelationships;
        }

        public bool AreEnemies(string factionA, string factionB)
        {
            var pair = new FactionPair(factionA, factionB);
            if (!_relationships.TryGetValue(pair, out var relationshipValue))
            {
                return false;
            }

            return relationshipValue <= EnemyThreshold;
        }
    }
}

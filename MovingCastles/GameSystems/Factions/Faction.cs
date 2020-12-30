using System.Collections.Generic;

namespace MovingCastles.GameSystems.Factions
{
    public static class Faction
    {
        public const string None = "";
        public const string Player = "Player";
        public const string Goblins = "Goblins";

        public static Dictionary<FactionPair, int> DefaultRelationships { get; } =
            new Dictionary<FactionPair, int>
            {
                { new FactionPair(Player, Goblins), -1000 },
            };
    }
}

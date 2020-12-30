namespace MovingCastles.GameSystems.Factions
{
    public interface IFactionMaster
    {
        bool AreEnemies(string factionA, string factionB);
    }
}
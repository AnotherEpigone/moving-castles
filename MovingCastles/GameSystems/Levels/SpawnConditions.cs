namespace MovingCastles.GameSystems.Levels
{
    public enum Spawn
    {
        Default,
        Stairdown,
        StairUp,
        Door,
    }

    public record SpawnConditions(Spawn Spawn, int LandmarkId);
}

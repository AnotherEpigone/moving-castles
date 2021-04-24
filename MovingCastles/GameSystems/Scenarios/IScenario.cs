using Troschuetz.Random;

namespace MovingCastles.GameSystems.Scenarios
{

    /// <summary>
    /// A map encounter which presents options to the player.
    /// This could represent navigating a castle-mode situation, or dialoging with an NPC.
    /// </summary>
    public interface IScenario
    {
        string Id { get; }

        // Sets up a new encounter with this scenario using current rng/stats
        // returns the first step of a new encounter
        IScenarioStep Encounter(IDungeonMaster dungeonMaster, IGenerator rng);
    }
}
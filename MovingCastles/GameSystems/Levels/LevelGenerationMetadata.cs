using GoRogue;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Levels
{
    public record LevelGenerationMetadata
    {
        public LevelGenerationMetadata()
        {
            Areas = new Dictionary<string, Rectangle>();
        }

        public Dictionary<string, Rectangle> Areas { get; }
    }
}

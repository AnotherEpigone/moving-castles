using MovingCastles.Serialization.Map;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Levels
{
    /// <summary>
    /// A set of Levels that are logically linked
    /// </summary>
    public class Structure
    {
        private readonly Dictionary<string, Level> _generatedLevels;
        private readonly Dictionary<string, MapState> _serializedLevels;
        private readonly Dictionary<string, ILevelGenerator> _generators;

        public Structure()
        {
            _generatedLevels = new Dictionary<string, Level>();
            _serializedLevels = new Dictionary<string, MapState>();
            _generators = new Dictionary<string, ILevelGenerator>();
        }
    }
}

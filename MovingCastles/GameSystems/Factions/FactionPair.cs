using System;
using System.Diagnostics;

namespace MovingCastles.GameSystems.Factions
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class FactionPair : IEquatable<FactionPair>
    {
        private readonly string _first;
        private readonly string _second;

        public FactionPair(string first, string second)
        {
            if (string.CompareOrdinal(first, second) > 0)
            {
                _first = first;
                _second = second;
            }
            else
            {
                _first = second;
                _second = first;
            }
        }

        public bool Equals(FactionPair other) => _first == other._first && _second == other._second;

        public override bool Equals(object obj) => obj is FactionPair pair && Equals(pair);

        public override int GetHashCode() => HashCode.Combine(_first, _second);

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(FactionPair)}: {_first} + {_second}");
            }
        }
    }
}

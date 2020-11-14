using MovingCastles.Entities;
using MovingCastles.GameSystems.Player;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Levels
{
    public interface ILevelGenerator
    {
        string Id { get; }

        Level Generate(int seed, PlayerInfo playerInfo);

        Level Generate(int seed, IEnumerable<McEntity> entities);
    }
}

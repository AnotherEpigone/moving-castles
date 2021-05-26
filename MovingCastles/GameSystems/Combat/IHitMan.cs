using MovingCastles.Entities;
using Troschuetz.Random;

namespace MovingCastles.GameSystems.Combat
{
    public interface IHitMan
    {
        HitResult Get(McEntity attacker, McEntity defender, IGenerator rng);
    }
}
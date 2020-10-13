using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;

namespace MovingCastles.GameSystems.Spells
{
    public interface ISpellEffect
    {
        void Apply(McEntity caster, McEntity target, ILogManager logManager);
    }
}
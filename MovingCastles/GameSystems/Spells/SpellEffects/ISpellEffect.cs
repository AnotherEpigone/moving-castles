using GoRogue;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;

namespace MovingCastles.GameSystems.Spells.SpellEffects
{
    public interface ISpellEffect
    {
        void Apply(McEntity caster, DungeonMap map, Coord targetCoord, ILogManager logManager);
    }
}
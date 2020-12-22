using GoRogue;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;

namespace MovingCastles.GameSystems.Spells.SpellEffects
{
    public interface ISpellEffect
    {
        void Apply(
            McEntity caster,
            SpellTemplate spell,
            DungeonMap map,
            HitResult hitResult,
            Coord targetCoord,
            ILogManager logManager);
    }
}
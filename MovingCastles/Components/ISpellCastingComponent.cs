using MovingCastles.GameSystems.Spells;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public interface ISpellCastingComponent
    {
        List<SpellTemplate> Spells { get; }
    }
}
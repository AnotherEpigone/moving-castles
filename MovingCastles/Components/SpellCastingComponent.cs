using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.GameSystems.Spells;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public class SpellCastingComponent : IGameObjectComponent, ISpellCastingComponent
    {
        public SpellCastingComponent(params SpellTemplate[] spells)
        {
            Spells = new List<SpellTemplate>(spells);
        }

        public IGameObject Parent { get; set; }

        public List<SpellTemplate> Spells { get; }
    }
}

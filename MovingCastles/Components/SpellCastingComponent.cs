using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems.Spells;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.Components
{
    public class SpellCastingComponent : IGameObjectComponent, ISpellCastingComponent, ISerializableComponent
    {
        public SpellCastingComponent(string state)
        {
            var spellTemplateIds = JsonConvert.DeserializeObject<List<string>>(state);
            Spells = spellTemplateIds.ConvertAll(id => SpellAtlas.SpellsById[id]);
        }

        public SpellCastingComponent(params SpellTemplate[] spells)
        {
            Spells = new List<SpellTemplate>(spells);
        }

        public IGameObject Parent { get; set; }

        public List<SpellTemplate> Spells { get; }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(SpellCastingComponent),
            State = JsonConvert.SerializeObject(Spells.Select(s => s.Id).ToList()),
        };
    }
}

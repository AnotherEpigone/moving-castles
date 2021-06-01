using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Components.ItemComponents;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.Serialization;
using Newtonsoft.Json;
using Troschuetz.Random;

namespace MovingCastles.Components
{
    public class MeleeAttackerComponent : IGameObjectComponent, IMeleeAttackerComponent, ISerializableComponent
    {
        private readonly int _unarmedDamage;

        public MeleeAttackerComponent(SerializedObject state)
        {
            _unarmedDamage = JsonConvert.DeserializeObject<int>(state.Value);
        }

        public MeleeAttackerComponent(int unarmedDamage)
        {
            _unarmedDamage = unarmedDamage;
        }

        public IGameObject Parent { get; set; }

        public float GetDamage(IGenerator rng)
        {
            if (!(Parent is McEntity mcParent))
            {
                return _unarmedDamage;
            }

            var weapon = mcParent.GetGoRogueComponent<IEquippedMeleeWeaponComponent>();
            if (weapon == null)
            {
                return _unarmedDamage;
            }

            return weapon.Damage.Roll(rng);
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(MeleeAttackerComponent),
            State = JsonConvert.SerializeObject(_unarmedDamage),
        };
    }
}

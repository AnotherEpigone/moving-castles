using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using MovingCastles.Components.Serialization;
using Newtonsoft.Json;

namespace MovingCastles.Components
{
    public class MeleeAttackerComponent : IGameObjectComponent, IMeleeAttackerComponent, ISerializableComponent
    {
        private readonly int _damage;

        public MeleeAttackerComponent(string state)
        {
            _damage = JsonConvert.DeserializeObject<int>(state);
        }

        public MeleeAttackerComponent(int damage)
        {
            _damage = damage;
        }

        public IGameObject Parent { get; set; }

        public float GetDamage()
        {
            return _damage;
        }

        public ComponentSerializable GetSerializable() => new ComponentSerializable()
        {
            Id = nameof(MeleeAttackerComponent),
            State = JsonConvert.SerializeObject(_damage),
        };
    }
}

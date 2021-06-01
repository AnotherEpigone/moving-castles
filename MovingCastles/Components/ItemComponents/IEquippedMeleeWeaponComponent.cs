using GoRogue.DiceNotation;
using MovingCastles.Components.Serialization;

namespace MovingCastles.Components.ItemComponents
{
    public interface IEquippedMeleeWeaponComponent : ISerializableComponent
    {
        IDiceExpression Damage { get; }

        int SpeedMod { get; }
    }
}

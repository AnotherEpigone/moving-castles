using GoRogue.DiceNotation;
using MovingCastles.Components.Effects;
using MovingCastles.Components.Serialization;

namespace MovingCastles.Components.ItemComponents
{
    public interface IEquippedMeleeWeaponComponent : ISerializableComponent, IDescribableEffect
    {
        IDiceExpression Damage { get; }

        int SpeedMod { get; }
    }
}

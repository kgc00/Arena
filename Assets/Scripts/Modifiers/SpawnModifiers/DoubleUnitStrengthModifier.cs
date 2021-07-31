using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class DoubleUnitStrengthModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.AttackPower;
        public override string DisplayText() => "2x Strength";
        public override UnitModifierType ModifierType { get; protected set; } = UnitModifierType.StrengthDouble;

        public override void Handle() {
            Model.statsData.strength *= 2;
            base.Handle();
        }
    }
}
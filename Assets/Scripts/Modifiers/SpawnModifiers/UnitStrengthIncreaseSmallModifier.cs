using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class UnitStrengthIncreaseSmallModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.AttackPower;
        public override string DisplayText() => "Attack+";
        public override UnitModifierType ModifierType { get; protected set; } = UnitModifierType.StrengthIncreaseSmall;

        public override void Handle() {
            Model.statsData.strength += 5;
            base.Handle();
        }
    }
}
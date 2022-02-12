using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class UnitStrengthIncreaseMediumModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.AttackPower;
        public override string DisplayText() => "Attack++";
        public override UnitModifierType ModifierType { get; protected set; } = UnitModifierType.StrengthIncreaseMedium;

        public override void Handle() {
            Model.statsData.strength += 15;
            base.Handle();
        }
    }
}
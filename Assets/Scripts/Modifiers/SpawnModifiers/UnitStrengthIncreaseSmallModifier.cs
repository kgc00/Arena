using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class UnitStrengthIncreaseSmallModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.AttackPower;
        public override string DisplayText() => "Attack+";

        public override void Handle() {
            Model.statsData.strength += 0.25f;
            base.Handle();
        }
    }
}
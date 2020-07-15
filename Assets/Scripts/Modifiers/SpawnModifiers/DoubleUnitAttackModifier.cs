using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class DoubleUnitAttackModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.AttackPower;
        public override string DisplayText() => "2x Attack";

        public override void Handle() {
            Model.statsData.strength *= 2;
            base.Handle();
        }
    }
}
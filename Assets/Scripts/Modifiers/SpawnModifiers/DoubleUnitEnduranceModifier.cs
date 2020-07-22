using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class DoubleUnitEnduranceModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.Health;
        public override string DisplayText() => "2x Endurance";

        public override void Handle() {
            Model.statsData.endurance *= 2;
            base.Handle();
        }
    }
}
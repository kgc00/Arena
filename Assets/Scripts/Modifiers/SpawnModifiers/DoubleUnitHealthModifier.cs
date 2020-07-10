using Data;

namespace Modifiers.SpawnModifiers {
    public class DoubleUnitHealthModifier : UnitModifier {
        public new string IconAssetPath = AssetPaths.Icons.Health;
        public override void Handle() {
            Model.health.maxHp *= 2;
            base.Handle();
        }
    }
}
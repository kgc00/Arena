using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class DoubleUnitHealthModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.Health;

        public override void Handle() {
            Model.health.maxHp *= 2;
            base.Handle();
        }
    }
}
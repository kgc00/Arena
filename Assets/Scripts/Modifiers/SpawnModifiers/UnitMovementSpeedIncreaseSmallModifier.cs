using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class UnitMovementSpeedIncreaseSmallModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.MovementSpeed;
        public override string DisplayText() => "+15 Speed";

        public override void Handle() {
            Model.statsData.movementSpeed += 15;
            base.Handle();
        }
    }
}
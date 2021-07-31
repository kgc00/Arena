using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class DoubleUnitMovementSpeedModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.MovementSpeed;
        public override string DisplayText() => "2x Speed";
        public override UnitModifierType ModifierType { get; protected set; } = UnitModifierType.MovementSpeedDouble;

        public override void Handle() {
            Model.statsData.movementSpeed *= 2;
            base.Handle();
        }
    }
}
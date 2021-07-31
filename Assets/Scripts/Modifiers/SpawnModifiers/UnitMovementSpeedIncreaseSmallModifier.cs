using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class UnitMovementSpeedIncreaseSmallModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.MovementSpeed;
        public override string DisplayText() => "Speed+";
        public override UnitModifierType ModifierType { get; protected set; } = UnitModifierType.MovementSpeedIncreaseSmall;

        public override void Handle() {
            Model.statsData.movementSpeed += 15;
            base.Handle();
        }
    }
}
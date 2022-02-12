using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class UnitEnduranceIncreaseMediumModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.Health;
        public override string DisplayText() => "Endurance++";
        public override UnitModifierType ModifierType { get; protected set; } = UnitModifierType.EnduranceIncreaseMedium;

        public override void Handle() {
            Model.statsData.endurance += 15;
            base.Handle();
        }
    }
}
using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class UnitEnduranceIncreaseSmallModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.Health;
        public override string DisplayText() => "Endurance+";
        public override UnitModifierType ModifierType { get; protected set; } = UnitModifierType.EnduranceIncreaseSmall;

        public override void Handle() {
            Model.statsData.endurance += 5;
            base.Handle();
        }
    }
}
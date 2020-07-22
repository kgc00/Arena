using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class UnitEnduranceIncreaseSmallModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.MovementSpeed;
        public override string DisplayText() => "Endurance+";

        public override void Handle() {
            Model.statsData.endurance += 0.25f;
            base.Handle();
        }
    }
}
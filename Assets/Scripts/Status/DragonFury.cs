using Data.Types;

namespace Status {
    public class DragonFury : MonoStatus {
        protected override void EnableEffect() => Owner.StatsComponent.IncrementStat(StatType.Strength, Amount);
        protected override void DisableEffect() {
            Owner.StatsComponent.DecrementStat(StatType.Strength, Amount);
            base.DisableEffect();
        }
    }
}
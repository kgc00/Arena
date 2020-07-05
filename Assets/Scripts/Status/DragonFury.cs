using Stats;

namespace Status {
    public class DragonFury : MonoStatus {
        protected override void EnableEffect() => Owner.StatsComponent.IncrementStat(StatType.Intelligence, Amount);
        protected override void DisableEffect() {
            Owner.StatsComponent.DecrementStat(StatType.Intelligence, Amount);
            base.DisableEffect();
        }
    }
}
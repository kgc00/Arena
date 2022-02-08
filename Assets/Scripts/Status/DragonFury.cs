using Data.Types;

namespace Status {
    public class DragonFury : MonoStatus {
        public override StatusType Type { get; protected set; } = StatusType.DragonFury;
        protected override void EnableEffect() => Owner.StatsComponent.IncrementStat(StatType.Strength, Amount);
        public override void DisableEffect() {
            Owner.StatsComponent.DecrementStat(StatType.Strength, Amount);
            base.DisableEffect();
        }
    }
}
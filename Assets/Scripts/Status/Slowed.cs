using System;
using Data.Types;

namespace Status {
    public class Slowed : MonoStatus {
        public override StatusType Type { get; protected set; } = StatusType.Slowed;
        protected override void EnableEffect() {
            Owner.StatsComponent.DecrementStat(StatType.MovementSpeed, Amount);
        }

        protected override void DisableEffect() {
            Owner.StatsComponent.IncrementStat(StatType.MovementSpeed, Amount);
            base.DisableEffect();
        }
    }
}
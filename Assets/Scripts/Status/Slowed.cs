using System;
using Data.Types;
using Stats;

namespace Status {
    public class Slowed : MonoStatus {
        protected override void EnableEffect() {
            Owner.StatsComponent.DecrementStat(StatType.MovementSpeed, Amount);
        }

        protected override void DisableEffect() {
            Owner.StatsComponent.IncrementStat(StatType.MovementSpeed, Amount);
            base.DisableEffect();
        }
    }
}
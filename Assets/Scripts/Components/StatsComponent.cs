using Data.StatData;
using Data.Stats;
using Data.Types;
using Units;
using UnityEngine;

namespace Components {
    public class StatsComponent : MonoBehaviour {
        public Unit Owner { get; private set; }
        public Stats Stats { get; private set; }
        public StatsComponent Initialize(Unit unit, StatsData data) {
            Owner = unit;
            Stats = new Stats(data);
            return this;
        }

        public void UpdateModel(StatsData data) {
            SetStatValue(StatType.Endurance, data.endurance);
            SetStatValue(StatType.Strength, data.strength);
            SetStatValue(StatType.MovementSpeed, data.movementSpeed);
        }
        
        public Statistic StatFromEnum(StatType type) => (Statistic) Stats.GetType().GetProperty(type.ToString())?.GetValue(Stats, null);
        public Statistic IncrementStat(StatType type, float value) => SetStatValue(type, Mathf.Abs(value));

        public Statistic DecrementStat(StatType type, float value) => SetStatValue(type, -Mathf.Abs(value));
        private Statistic SetStatValue(StatType type, float value) {
            var stat = StatFromEnum(type);
            stat.Value += value;
            return stat;
        }
        
        public float GetAbilityCooldown(float baseCooldown, float minimumAbilityCooldown = 0f) {
            var coolDownModifier = Mathf.Max(Stats.Intelligence.Value, 1) - 1;
            var cooldownReduction = 1 - coolDownModifier / 9; // intel of 10 = no cooldown
            return Mathf.Max(minimumAbilityCooldown, baseCooldown * cooldownReduction);
        }
    }
}
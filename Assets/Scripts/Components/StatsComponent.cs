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
            SetStatValue(StatType.Intelligence, data.intelligence);
            SetStatValue(StatType.Agility, data.agility);
        }
        
        public Statistic StatFromEnum(StatType type) => (Statistic) Stats.GetType().GetProperty(type.ToString())?.GetValue(Stats, null);
        public Statistic IncrementStat(StatType type, int value) => SetStatValue(type, Mathf.Abs(value));
        public Statistic DecrementStat(StatType type, int value) => SetStatValue(type, -Mathf.Abs(value));
        private Statistic SetStatValue(StatType type, int value) {
            var stat = StatFromEnum(type);
            stat.Value += value;
            return stat;
        }
    }
}
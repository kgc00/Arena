﻿using Data.StatData;
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
        public Statistic StatFromEnum(StatType type) => (Statistic) Stats.GetType().GetProperty(type.ToString())?.GetValue(Stats, null);
        public Statistic IncrementStat(StatType type, float value) => ModifyStatValue(type, Mathf.Abs(value));

        public Statistic DecrementStat(StatType type, float value) => ModifyStatValue(type, -Mathf.Abs(value));
        private Statistic ModifyStatValue(StatType type, float value) {
            var stat = StatFromEnum(type);
            stat.Value += value;
            return stat;
        }
    }
}
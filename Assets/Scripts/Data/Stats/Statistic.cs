using Data.Types;

namespace Data.Stats {
    public class Statistic {
        public Statistic (float initialValue, StatType type) {
            Value = initialValue;
            Type = type;
        }
        public float Value { get; set; }
        public StatType Type { get; set; }
    }
}
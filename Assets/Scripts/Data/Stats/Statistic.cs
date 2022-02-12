using Data.Types;

namespace Data.Stats {
    public class Statistic {
        public Statistic (int initialValue, StatType type) {
            Value = initialValue;
            Type = type;
        }
        public int Value { get; set; }
        public StatType Type { get; set; }
    }
}
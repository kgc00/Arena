using System.Collections;
using Data.StatData;
using Data.Types;

namespace Data.Stats {
    public class Stats { 
        
                    /// average stats ///
        public Statistic Agility { get; private set; } // 1
        public Statistic Strength { get; private set; } // 1
        public Statistic Intelligence { get; private set; } // 1
        public Statistic Endurance { get; private set; } // 1
        public Statistic MovementSpeed { get; private set; } // 100

        public Statistic StatFromEnum(StatType type) {
            return (Statistic) GetType().GetProperty(type.ToString())?.GetValue(this, null);
        }
        
        
        // for debugging
        public Stats() {
            Agility = new Statistic(1, StatType.Agility);
            Strength = new Statistic(1, StatType.Strength);
            Intelligence = new Statistic(1, StatType.Intelligence);
            Endurance = new Statistic(1, StatType.Endurance);
            MovementSpeed = new Statistic(1, StatType.MovementSpeed);
        }

        public Stats(Statistic agility, Statistic strength, Statistic intelligence, Statistic endurance, Statistic movementSpeed) {
            Agility = agility;
            Strength = strength;
            Intelligence = intelligence;
            Endurance = endurance;
            MovementSpeed = movementSpeed;
        }
        public Stats(StatsData data) {
            Agility = new Statistic(data.agility, StatType.Agility);
            Strength = new Statistic(data.strength, StatType.Strength);
            Intelligence = new Statistic(data.intelligence, StatType.Intelligence);
            Endurance = new Statistic(data.endurance, StatType.Endurance);
            MovementSpeed = new Statistic(data.movementSpeed, StatType.MovementSpeed);
        }
    }
}
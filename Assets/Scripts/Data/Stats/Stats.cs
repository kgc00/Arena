using System.Collections;
using Data.StatData;
using Data.Types;
using UnityEngine;

namespace Data.Stats {
    public class Stats { 
        
                    /// average stats ///
        public Statistic Strength { get; private set; } // 1
        public Statistic Endurance { get; private set; } // 1
        public Statistic MovementSpeed { get; private set; } // 100
        public Statistic Intelligence { get; private set; } // 1
        public Statistic Agility { get; private set; } // 1

        public Statistic StatFromEnum(StatType type) {
            return (Statistic) GetType().GetProperty(type.ToString())?.GetValue(this, null);
        }
        
        
        // for debugging
        public Stats() {
            Strength = new Statistic(1, StatType.Strength);
            Endurance = new Statistic(1, StatType.Endurance);
            MovementSpeed = new Statistic(1, StatType.MovementSpeed);
            Intelligence = new Statistic(1, StatType.Intelligence);
            Agility = new Statistic(1, StatType.Agility);
        }
        
        public Stats(StatsData data) {
            Strength = new Statistic(data.strength, StatType.Strength);
            Endurance = new Statistic(data.endurance, StatType.Endurance);
            MovementSpeed = new Statistic(data.movementSpeed, StatType.MovementSpeed);
            Intelligence = new Statistic(data.intelligence, StatType.Intelligence);
            Agility = new Statistic(data.agility, StatType.Agility);
        }
    }
}
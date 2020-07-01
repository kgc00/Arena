namespace Stats {
    public class Stats { 
        
                    /// average stats ///
        public Statistic Agility { get; private set; } // 1
        public Statistic Strength { get; private set; } // 1
        public Statistic Intelligence { get; private set; } // 1
        public Statistic Endurance { get; private set; } // 1
        public Statistic MovementSpeed { get; private set; } // 100

        public Statistic StatFromEnum(StatType type) {
            return (Statistic) this.GetType().GetProperty(type.ToString()).GetValue(this, null);
        }
        
        
        // for debugging
        public Stats() {
            Agility = new Statistic(1);
            Strength = new Statistic(1);
            Intelligence = new Statistic(1);
            Endurance = new Statistic(1);
            MovementSpeed = new Statistic(1);
        }

        public Stats(Statistic agility, Statistic strength, Statistic intelligence, Statistic endurance, Statistic movementSpeed) {
            Agility = agility;
            Strength = strength;
            Intelligence = intelligence;
            Endurance = endurance;
            MovementSpeed = movementSpeed;
        }
        public Stats(StatsData data) {
            Agility = new Statistic(data.Agility);
            Strength = new Statistic(data.Strength);
            Intelligence = new Statistic(data.Intelligence);
            Endurance = new Statistic(data.Endurance);
            MovementSpeed = new Statistic(data.MovementSpeed);
        }
    }
}
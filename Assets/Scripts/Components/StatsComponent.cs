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
        public Statistic IncrementStat(StatType type, float value) => SetStatValue(type, Mathf.Abs(value));
        public Statistic DecrementStat(StatType type, float value) => SetStatValue(type, -Mathf.Abs(value));
        private Statistic SetStatValue(StatType type, float value) {
            var stat = StatFromEnum(type);
            stat.Value += value;
            return stat;
        }
        
        public float GetAbilityCooldown(float baseCooldown, float minimumAbilityCooldown = 0f) {
            var cooldownModifier = Mathf.Max(Stats.Intelligence.Value, 1) - 1;
            var cooldownReduction = 1 - cooldownModifier / 99; // intel of 100 = no cooldown
            return Mathf.Max(minimumAbilityCooldown, baseCooldown * cooldownReduction);
        }
        
        public int GetMaxHealth(float baseMaxHp) {
            var healthModifier = Mathf.Max(Stats.Endurance.Value, 1) - 1;
            var healthIncrease = healthModifier / 99; // Endurance of 100 = double hp
            return (int) (baseMaxHp + baseMaxHp  * healthIncrease);
        }

        public float GetAoERadius(int baseAreaOfEffectRadius) {
            var aoeRadiusModifier = Mathf.Max(Stats.Intelligence.Value, 1) - 1;
            var aoeRadiusIncrease = aoeRadiusModifier / 150; // intel of 100 = 150% radius
            return baseAreaOfEffectRadius + baseAreaOfEffectRadius  * aoeRadiusIncrease;
        }
        
        public float GetDamage(float baseDamage) {
            var damageModifier = Mathf.Max(Stats.Strength.Value, 1) - 1;
            var damageIncrease = damageModifier / 99; // strength of 100 = double damage
            return baseDamage + baseDamage  * damageIncrease;
        }
    }
}
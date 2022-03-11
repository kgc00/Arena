using System.Collections.Generic;
using System.Linq;
using Data.AbilityData;
using Data.StatData;
using UnityEngine;

namespace Data.UnitData {
    public static class UnitDataExtensions {
        /// <summary>
        /// Makes a copy of all variables passed by reference
        /// to avoid overwriting data on the original object
        /// </summary>
        /// <param name="data"></param>
        /// <returns>An instance of UnitData with the same values as the input</returns>
        public static UnitData CreateInstance(this UnitData data) {
            var instance = ScriptableObject.CreateInstance<UnitData>();
            AssignAbilities(data, instance);
            instance.experience = new ExperienceData(data.experience);
            instance.health = new HealthData(data.health);
            instance.state = data.state;
            instance.visualAssets = new VisualAssets(data.visualAssets);
            instance.statsData = new StatsData(data.statsData);
            instance.fundsData = new FundsData(data.fundsData);
            instance.poolKey = data.poolKey;
            return instance;
        }

        private static void AssignAbilities(UnitData data, UnitData instance) {
            // todo - handle case where there are no abilities
            instance.abilities = data.abilities.ConvertAll(x => x)?.ToList();

            // maybe i'll use later
            // instance.abilities = new List<AbilityData.AbilityData>();
            // foreach (var abilityData in data.abilities) {
            //     if (abilityData is AttackAbilityData attackAbilityData)
            //         instance.abilities.Add(attackAbilityData.CreateInstance());
            //     else if (abilityData is BuffAbilityData buffAbilityData)
            //         instance.abilities.Add(buffAbilityData.CreateInstance());
            //     else if (abilityData is MovementAttackAbilityData movementAttackAbilityData)
            //         instance.abilities.Add(movementAttackAbilityData.CreateInstance());
            //     else instance.abilities.Add(abilityData.CreateInstance());
            // }
        }
    }
}
using System.Linq;
using Stats.Data;
using UnityEngine;

namespace Units.Data {
    public static class UnitDataExtensions{
        /// <summary>
        /// Makes a copy of all variables passed by reference
        /// to avoid overwriting data on the original object
        /// </summary>
        /// <param name="data"></param>
        /// <returns>An instance of UnitData with the same values as the input</returns>
        public static UnitData CreateInstance(this UnitData data) {
            var instance = ScriptableObject.CreateInstance<UnitData>();
            instance.abilities = data.abilities.ConvertAll(x => x).ToList();
            instance.experience = new ExperienceData(data.experience);
            instance.health = new HealthData(data.health);
            instance.state = data.state;
            instance.visualAssets = new VisualAssets(data.visualAssets);
            return instance;
        }
    }
}
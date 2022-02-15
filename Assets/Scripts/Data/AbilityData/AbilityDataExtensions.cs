using UnityEngine;

namespace Data.AbilityData {
    public static class AbilityDataExtensions {
        public static AbilityData CreateInstance(this AbilityData data) {
            var instance = ScriptableObject.CreateInstance<AbilityData>();
            // use child classes' createinstance method
            return instance;
        }
    }
}
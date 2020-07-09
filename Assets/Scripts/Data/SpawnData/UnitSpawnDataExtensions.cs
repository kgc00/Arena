using UnityEngine;

namespace Data.SpawnData {
    public static class UnitSpawnDataExtensions {
        public static UnitSpawnData CreateInstance(this UnitSpawnData data) {
            var instance = ScriptableObject.CreateInstance<UnitSpawnData>();
            instance.Amount = data.Amount;
            instance.Unit = data.Unit;
            return instance;
        }

    }
}
using System.Linq;
using UnityEngine;

namespace Data.SpawnData {
    public static class UnitSpawnDataExtensions {
        public static UnitSpawnData CreateInstance(this UnitSpawnData data) {
            var instance = ScriptableObject.CreateInstance<UnitSpawnData>();
            instance.Amount = data.Amount;
            instance.Unit = data.Unit;
            instance.modifiers = data.modifiers.ConvertAll(x => x).ToList();
            return instance;
        }
    }
}
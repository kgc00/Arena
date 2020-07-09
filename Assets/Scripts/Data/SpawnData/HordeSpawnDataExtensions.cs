using System.Linq;
using UnityEngine;

namespace Data.SpawnData {
    public static class HordeSpawnDataExtensions {
        public static HordeSpawnData CreateInstance(this HordeSpawnData data) {
            var instance = ScriptableObject.CreateInstance<HordeSpawnData>();
            instance.Waves = data.Waves.ConvertAll(x => x).ToList();
            return instance;
        }
    }
}
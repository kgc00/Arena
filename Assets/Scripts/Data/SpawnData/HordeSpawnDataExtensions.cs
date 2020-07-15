using System.Linq;
using UnityEngine;

namespace Data.SpawnData {
    public static class HordeSpawnDataExtensions {
        public static HordeSpawnData CreateInstance(this HordeSpawnData data) {
            var instance = ScriptableObject.CreateInstance<HordeSpawnData>();
            instance.Waves = data.Waves.ConvertAll(x => x.CreateInstance()).ToList();
            return instance;
        }
        
        public static void Update(this HordeSpawnData current, HordeSpawnData newModel) {
            current.Waves.Clear();
            foreach (var waveSpawnData in newModel.Waves) {
                current.Waves.Add(waveSpawnData.CreateInstance());
            }
        }
    }
}
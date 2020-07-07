using System.Linq;
using UnityEngine;

namespace Spawner.Data {
    public static class WaveSpawnDataExtensions {
        public static WaveSpawnData CreateInstance(this WaveSpawnData data) {
            var instance = ScriptableObject.CreateInstance<WaveSpawnData>();
            instance.wave = data.wave.ConvertAll(x => x).ToList();
            return instance;
        }
    }
}
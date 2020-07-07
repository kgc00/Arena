using System.Linq;
using UnityEngine;

namespace Spawner.Data {
    public static class WaveSpawnDataExtensions {
        public static WaveSpawnData CreateInstance(this WaveSpawnData data) {
            var instance = ScriptableObject.CreateInstance<WaveSpawnData>();
            instance.Wave = data.Wave.ConvertAll(x => x).ToList();
            return instance;
        }
    }
}
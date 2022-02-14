using System.Linq;
using UnityEngine;

namespace Data.SpawnData {
    public static class WaveSpawnDataExtensions {
        public static WaveSpawnData CreateInstance(this WaveSpawnData data) {
            var instance = ScriptableObject.CreateInstance<WaveSpawnData>();
            instance.wave = data.wave.ConvertAll(x => x.CreateInstance()).ToList();
            var mods = data.modifiers.ConvertAll(x => x).ToList();
            instance.modifiers = mods;
            return instance;
        }
    }
}
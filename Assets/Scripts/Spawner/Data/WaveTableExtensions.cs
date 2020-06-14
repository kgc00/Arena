using System.Linq;
using UnityEngine;

namespace Spawner.Data {
    public static class WaveTableExtensions {
        public static WaveTable CreateInstance(this WaveTable data) {
            var instance = ScriptableObject.CreateInstance<WaveTable>();
            instance.Wave = data.Wave.ConvertAll(x => x).ToList();
            instance.WaveNumber = data.WaveNumber;
            return instance;
        }
    }
}
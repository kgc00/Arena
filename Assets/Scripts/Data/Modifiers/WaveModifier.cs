using Common;
using Data.SpawnData;

namespace Data.Modifiers {
    public class WaveModifier : ScriptableObjectModifier<WaveSpawnData> {
        public override ScriptableObjectModifier<WaveSpawnData> InitializeModifier(WaveSpawnData data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
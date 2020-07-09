using Data.SpawnData;

namespace Units.Modifiers {
    public class WaveTableModifier : ScriptableObjectModifier<WaveSpawnData> {
        public override ScriptableObjectModifier<WaveSpawnData> InitializeModifier(WaveSpawnData data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
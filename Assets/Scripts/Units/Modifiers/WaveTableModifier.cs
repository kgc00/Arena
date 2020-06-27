using Spawner.Data;

namespace Units.Modifiers {
    public class WaveTableModifier : ScriptableObjectModifier<WaveTable> {
        public override ScriptableObjectModifier<WaveTable> InitializeModifier(WaveTable data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
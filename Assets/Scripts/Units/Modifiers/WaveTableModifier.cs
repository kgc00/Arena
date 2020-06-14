using Spawner.Data;

namespace Units.Modifiers {
    public class WaveTableModifier : ScrObjModifier<WaveTable> {
        public override ScrObjModifier<WaveTable> InitializeModifier(WaveTable data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
using Common;
using Data.UnitData;

namespace Modifiers.SpawnModifiers {
    public class UnitModifier : ScriptableObjectModifier<UnitData> {
        
        public override ScriptableObjectModifier<UnitData> InitializeModifier(UnitData data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
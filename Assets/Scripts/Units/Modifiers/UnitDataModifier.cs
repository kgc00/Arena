using Units.Data;

namespace Units.Modifiers {
    public class UnitDataModifier : ScriptableObjectModifier<UnitData> {
        public override ScriptableObjectModifier<UnitData> InitializeModifier(UnitData data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
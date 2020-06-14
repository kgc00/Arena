using Units.Data;

namespace Units.Modifiers {
    public class UnitDataModifier : ScrObjModifier<UnitData> {
        public override ScrObjModifier<UnitData> InitializeModifier(UnitData data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
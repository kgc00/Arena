using System;
using Common;

namespace Data.Modifiers {
    [Serializable]
    public class UnitModifier : ScriptableObjectModifier<UnitData.UnitData> {
        public override ScriptableObjectModifier<UnitData.UnitData> InitializeModifier(UnitData.UnitData data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
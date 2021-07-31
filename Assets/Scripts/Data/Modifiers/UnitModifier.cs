using System;
using Common;

namespace Data.Modifiers {
    [Serializable]
    public class UnitModifier : ScriptableObjectModifier<UnitData.UnitData> {
        public virtual UnitModifierType ModifierType { get; protected set; } = UnitModifierType.BaseModifier;
        public override ScriptableObjectModifier<UnitData.UnitData> InitializeModifier(UnitData.UnitData data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
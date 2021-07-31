using System;
using Common;
using Data.SpawnData;

namespace Data.Modifiers {
    [Serializable]
    public class WaveModifier : ScriptableObjectModifier<WaveSpawnData> {
        public virtual WaveModifierType ModifierType { get; protected set; } = WaveModifierType.BaseModifier;
        public override ScriptableObjectModifier<WaveSpawnData> InitializeModifier(WaveSpawnData data)
        {
            base.InitializeModifier(data);
            Model = data;
            return this;
        }
    }
}
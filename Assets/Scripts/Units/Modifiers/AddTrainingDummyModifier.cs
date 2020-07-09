using Data.SpawnData;
using Data.Types;
using UnityEngine;

namespace Units.Modifiers {
    public class AddTrainingDummyModifier : WaveTableModifier {
        public override void Handle() {
            var instance = ScriptableObject.CreateInstance<UnitSpawnData>();
            instance.Amount = 1;
            instance.Unit = UnitType.TrainingDummy;
            Model.wave.Add(instance);
            base.Handle();
        }
        
    }
}
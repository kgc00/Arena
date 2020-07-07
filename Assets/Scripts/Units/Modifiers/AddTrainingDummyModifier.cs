using Spawner.Data;
using UnityEngine;

namespace Units.Modifiers {
    public class AddTrainingDummyModifier : WaveTableModifier {
        public override void Handle() {
            var instance = ScriptableObject.CreateInstance<UnitSpawnData>();
            instance.Amount = 1;
            instance.Unit = Types.TrainingDummy;
            Model.Wave.Add(instance);
            base.Handle();
        }
        
    }
}
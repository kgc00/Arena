using Data.SpawnData;
using Data.Types;
using UnityEngine;

namespace Modifiers.SpawnModifiers {
    public class AddTrainingDummyModifier : WaveModifier {
        public override void Handle() {
            var instance = ScriptableObject.CreateInstance<UnitSpawnData>();
            instance.Amount = 1;
            instance.Unit = UnitType.TrainingDummy;
            Model.wave.Add(instance);
            base.Handle();
        }
    }
}
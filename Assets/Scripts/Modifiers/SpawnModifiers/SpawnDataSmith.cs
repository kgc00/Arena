using System.Collections.Generic;
using Data.Modifiers;
using Data.SpawnData;
using Data.UnitData;

namespace Modifiers.SpawnModifiers {
    public static class SpawnDataSmith {
        public static WaveSpawnData ModifyWaveData(WaveSpawnData instance, List<WaveModifier> modifications) {
            var root = new WaveModifier().InitializeModifier(instance);

            foreach (var m in modifications)
                root.Add(m.InitializeModifier(instance));

            // Debug.Log($"Modifer list is {modifiers.Count} items long");

            root.Handle();

            return instance;
        }

        public static UnitData ModifyUnitData(UnitData instance, List<UnitModifier> modifications) {
            var root = new UnitModifier().InitializeModifier(instance);

            foreach (var m in modifications) root.Add(m.InitializeModifier(instance));

            // Debug.Log($"Modifer list is {modifiers.Count} items long");

            root.Handle();

            return instance;
        }
    }
}
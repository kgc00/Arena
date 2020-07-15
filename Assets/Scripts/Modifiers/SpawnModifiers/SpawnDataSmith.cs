using System;
using System.Collections.Generic;
using Data.Modifiers;
using Data.SpawnData;
using Data.UnitData;
using TypeReferences;

namespace Modifiers.SpawnModifiers {
    public static class SpawnDataSmith {
        public static WaveSpawnData ModifyWaveData(WaveSpawnData model) {
            var root = new WaveModifier().InitializeModifier(model);

            foreach (var typeRef in model.modifiers)
                if (Activator.CreateInstance(typeRef) is WaveModifier mod)
                    root.Add(mod.InitializeModifier(model));

            // Debug.Log($"Modifer list is {modifiers.Count} items long");

            root.Handle();

            return model;
        }

        public static UnitData ModifyUnitData(UnitData model, List<ClassTypeReference> modifications) {
            var root = new UnitModifier().InitializeModifier(model);
            
            foreach (var typeRef in modifications)
                if (Activator.CreateInstance(typeRef) is UnitModifier mod)
                    root.Add(mod.InitializeModifier(model));

            // Debug.Log($"Modifer list is {modifiers.Count} items long");

            root.Handle();

            return model;
        }
    }
}
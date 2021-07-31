using System;
using System.Collections.Generic;
using Data.Modifiers;
using Data.SpawnData;
using Data.UnitData;

namespace Modifiers.SpawnModifiers {
    public static class SpawnDataSmith {
        public static WaveSpawnData ModifyWaveData(WaveSpawnData model) {
            var modifications = model.modifiers;
            var root = new WaveModifier().InitializeModifier(model);

            foreach (var type in modifications)
                root.Add(InitializedWaveModiferFromType(type, model));

            // Debug.Log($"Modifer list is {modifiers.Count} items long");

            root.Handle();

            return model;
        }

        private static WaveModifier InitializedWaveModiferFromType(WaveModifierType type, WaveSpawnData model) {
            var instance = type switch {
                WaveModifierType.BaseModifier => new WaveModifier(),
                WaveModifierType.AddTrainingDummy => new AddTrainingDummyModifier(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            instance.InitializeModifier(model);
            return instance;
        }

        public static UnitData ModifyUnitData(UnitData model, List<UnitModifierType> modifications) {
            var root = new UnitModifier().InitializeModifier(model);
            
            foreach (var type in modifications)
                root.Add(InitializedUnitModiferFromType(type, model));

            // Debug.Log($"Modifer list is {modifiers.Count} items long");

            root.Handle();

            return model;
        }

        public static UnitModifier InitializedUnitModiferFromType(UnitModifierType type, UnitData model) {
            var instance = type switch {
                UnitModifierType.BaseModifier => new UnitModifier(),
                UnitModifierType.EnduranceDouble => new DoubleUnitEnduranceModifier(),
                UnitModifierType.MovementSpeedDouble => new DoubleUnitMovementSpeedModifier(),
                UnitModifierType.StrengthDouble => new DoubleUnitStrengthModifier(),
                UnitModifierType.StrengthIncreaseMedium => new UnitStrengthIncreaseMediumModifier(),
                UnitModifierType.StrengthIncreaseSmall => new UnitStrengthIncreaseSmallModifier(),
                UnitModifierType.MovementSpeedIncreaseMedium => new UnitMovementSpeedIncreaseMediumModifier(),
                UnitModifierType.MovementSpeedIncreaseSmall => new UnitMovementSpeedIncreaseSmallModifier(),
                UnitModifierType.EnduranceIncreaseMedium => new UnitEnduranceIncreaseMediumModifier(),
                UnitModifierType.EnduranceIncreaseSmall => new UnitEnduranceIncreaseSmallModifier(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            instance.InitializeModifier(model);
            return instance;
        }
    }
}
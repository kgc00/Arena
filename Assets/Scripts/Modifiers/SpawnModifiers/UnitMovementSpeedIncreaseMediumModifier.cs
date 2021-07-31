﻿using Data;
using Data.Modifiers;

namespace Modifiers.SpawnModifiers {
    public class UnitMovementSpeedIncreaseMediumModifier : UnitModifier {
        public override string IconAssetPath() => AssetPaths.Icons.MovementSpeed;
        public override string DisplayText() => "Speed++";
        public override UnitModifierType ModifierType { get; protected set; } = UnitModifierType.MovementSpeedIncreaseMedium;

        public override void Handle() {
            Model.statsData.movementSpeed += 30;
            base.Handle();
        }
    }
}
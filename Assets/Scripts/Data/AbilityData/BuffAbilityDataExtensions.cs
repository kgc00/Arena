﻿using UnityEngine;

namespace Data.AbilityData {
    public static class BuffAbilityDataExtensions {
        public static BuffAbilityData CreateInstance(this BuffAbilityData data) {
            var instance = ScriptableObject.CreateInstance<BuffAbilityData>();
            instance.cooldown = data.cooldown;
            instance.description = data.description;
            instance.duration = data.duration;
            instance.force = data.force;
            instance.icon = data.icon;
            instance.modifiers = data.modifiers;
            instance.range = data.range;
            instance.type = data.type;
            instance.displayName = data.displayName;
            instance.energyCost = data.energyCost;
            instance.equipableModifiers = data.equipableModifiers;
            instance.indicatorType = data.indicatorType;
            instance.projectileSpeed = data.projectileSpeed;
            instance.startupTime = data.startupTime;
            instance.areaOfEffectRadius = data.areaOfEffectRadius;
            instance.AffectedFactions = data.AffectedFactions;
            instance.Duration = data.Duration;
            return instance;
        }
    }
}
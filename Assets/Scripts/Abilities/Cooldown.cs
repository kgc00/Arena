using System;
using UnityEngine;
using static Utils.MathHelpers;

namespace Abilities
{
    public class Cooldown {
        public float TimeLeft;
        public readonly float CooldownTime;
        public bool IsFrozen;
        public bool Freeze() => IsFrozen = true;
        public bool UnFreeze() => IsFrozen = false;
        public bool IsOnCooldown => TimeLeft > 0f;
        public const float DefaultTimeLeft = -1;

        public float SetOnCooldown() => TimeLeft = (float) CooldownTime;

        public Cooldown(float cooldownTime, float currentTimeLeft, bool currentIsfrozen) {
            CooldownTime = cooldownTime;
            if (currentTimeLeft != DefaultTimeLeft) {
                TimeLeft = currentTimeLeft;
            }
            IsFrozen = currentIsfrozen;
        }
        
        // Must be called from ability component's update loop
        public float UpdateCooldown(float deltaTime)
        {
            if (IsFrozen || !IsOnCooldown) return TimeLeft;
            // Debug.Log($"Cooldown time left: {TimeLeft}");
            TimeLeft = Clamp(TimeLeft -= deltaTime, 0, CooldownTime);
            return TimeLeft;
        }
    }
}
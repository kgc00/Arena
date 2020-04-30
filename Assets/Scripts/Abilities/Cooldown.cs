using System;
using UnityEngine;
using static Utils.MathHelpers;

namespace Abilities
{
    public class Cooldown
    {
        public float TimeLeft = 0f;
        public float? CooldownTime;
        public bool IsFrozen = false;
        public bool Freeze() => IsFrozen = true;
        public bool UnFreeze() => IsFrozen = false;
        public bool IsOnCooldown => TimeLeft > 0f;
        public float SetOnCooldown() => TimeLeft = (float) CooldownTime;

        public Cooldown(float cooldownTime)
        {
            CooldownTime = cooldownTime;
        }
        
        // Must be called from ability component's update loop
        public float UpdateCooldown(float deltaTime)
        {
            if (CooldownTime == null) throw new Exception("CooldownTime was never assigned");
            
            // Debug.Log($"Cooldown time left: {TimeLeft}");
            if (IsFrozen || !IsOnCooldown) return TimeLeft;
            
            return TimeLeft = Clamp(TimeLeft -= deltaTime, 0, (float)CooldownTime);
        }
    }
}
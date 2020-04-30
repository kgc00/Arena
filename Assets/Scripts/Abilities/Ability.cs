using System;
using Abilities.Data;
using Controls;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public float Range { get; protected set; }
        public int IndicatorType { get; set; }
        public float StartupTime { get; protected set; }
        public Cooldown Cooldown{ get; protected set; } 
        public Unit Owner;
        public abstract void Activate (Vector3 targetLocation);
        protected virtual void LateUpdate() => Cooldown.UpdateCooldown(Time.deltaTime);
    }
}
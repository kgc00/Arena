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
        public Cooldown Cooldown{ get; protected set; } = new Cooldown();
        public Unit Owner;
        public abstract void Activate (Vector3 targetLocation);
        protected virtual void LateUpdate() => Cooldown.UpdateCooldown(Time.deltaTime);
    }
}
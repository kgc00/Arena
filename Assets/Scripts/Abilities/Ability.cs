using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Data;
using Controls;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        // public AbilityData Model { get; private set; }
        public float Range { get; protected set; }
        public float Force { get; protected set; }
        public float ProjectileSpeed { get; protected set; }
        public int AreaOfEffectRadius { get; protected set; }
        public int IndicatorType { get; set; }
        public float StartupTime { get; protected set; }
        public Cooldown Cooldown{ get; protected set; } 
        public Unit Owner;
        public static Action<Unit, Ability> onAbilityActivationFinished { get; set; } = delegate { };
        public List<Func<Vector3, IEnumerator>> OnActivation { get; set; }
        public abstract IEnumerator AbilityActivated(Vector3 targetLocation);
        
        // public List<Func<Vector3, IEnumerator>> OnAoEExecution { get; set; }
        // public abstract IEnumerator AbilityAoEExecuted(Vector3 targetLocation);
        
        protected virtual void LateUpdate() => Cooldown.UpdateCooldown(Time.deltaTime);

        // public virtual Ability Initialize(AbilityData model, Unit owner) => this;
        public abstract void ResetInstanceValues();
    }
}
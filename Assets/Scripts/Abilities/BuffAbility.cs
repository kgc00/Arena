using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Data;
using Enums;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class BuffAbility : Ability, IBuffUser
    {
        public BuffAbilityData Model { get; private set; }
        public float Duration { get; protected set; }
        public List<ControlType> AffectedFactions { get; protected set; }
        public virtual BuffAbility Initialize(BuffAbilityData data, Unit owner)
        {
            Owner = owner;
            Model = data;
            Range = data.range;
            Force = data.force;            
            Icon = data.icon;
            AreaOfEffectRadius = data.areaOfEffectRadius;
            AffectedFactions = data.AffectedFactions;
            Cooldown = new Cooldown(data.cooldown);
            StartupTime = data.startupTime;
            ProjectileSpeed = data.projectileSpeed;
            IndicatorType = data.indicatorType;
            Duration = data.Duration;
            OnActivation = new List<Func<Vector3, IEnumerator>>() {targetLocation => AbilityActivated(targetLocation)};
            return this;
        }
        
        public override void ResetInstanceValues()
        {
            if (Model == null || Owner == null)
            {
                Debug.Log("Model or Owner are not set and we cannot reset instance values");
                return;
            }

            Initialize(Model, Owner);
        }
    }
}
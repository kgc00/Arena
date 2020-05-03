using System.Collections.Generic;
using Abilities.AttackAbilities;
using Abilities.Data;
using Enums;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class AttackAbility : Ability, IDamageDealer {
        public virtual void OnAbilityConnected (GameObject targetedUnit){}
        public virtual void OnAbilityConnected (GameObject targetedUnit, GameObject projectile){}
        
        // public List<Action> ActivateBase = new List<System.Action<GameObject other, GameObject projectile>(){Activate};
        // public List<Action> OnAbilityConnectedBase = new List<System.Action<GameObject other, GameObject projectile>(){OnAbilityConnected};
        
        public float Damage { get; set;}
        public List<ControlType> AffectedFactions { get; protected set; }

        public virtual Ability Initialize(AttackAbilityData data, Unit owner)
        {
            Owner = owner;
            Damage = data.Damage;
            Range = data.range;
            AreaOfEffectRadius = data.areaOfEffectRadius;
            AffectedFactions = data.AffectedFactions;
            Cooldown = new Cooldown(data.cooldown);
            StartupTime = data.startupTime;
            IndicatorType = data.indicatorType;
            return this;
        }
    }
}
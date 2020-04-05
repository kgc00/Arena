﻿using System.Collections.Generic;
using Abilities.AttackAbilities;
using Abilities.Data;
using Enums;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class AttackAbility : Ability, IDamageDealer {
        public abstract void OnAbilityConnected (GameObject targetedUnit);
        public float Damage { get; protected set;}
        public List<ControlType> AffectedFactions { get; protected set; }
        public int AreaOfEffectRadius { get; protected set; }

        public virtual Ability Initialize(AttackAbilityData data, Unit owner)
        {
            Owner = owner;
            Damage = data.Damage;
            Range = data.range;
            AreaOfEffectRadius = data.areaOfEffectRadius;
            AffectedFactions = data.AffectedFactions;
            Cooldown.CooldownTime = data.cooldown;
            return this;
        }
    }
}
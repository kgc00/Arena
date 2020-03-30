using System.Collections.Generic;
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
        public int AreaOfEffectRadius { get; protected set; }
        public List<ControlType> AffectedTargets { get; protected set; }
        public abstract Ability Initialize(AttackAbilityData data, Unit owner);
    }
}
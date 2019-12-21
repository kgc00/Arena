using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Abilities
{
    public abstract class AttackAbility : Ability {
        [SerializeField] public float Damage { get; protected set; } 
        [SerializeField] public List<ControlType> affectedTargets;
        public abstract void OnAbilityConnected (GameObject targetedUnit);
    }
}
using System;
using System.Collections.Generic;
using Abilities.AttackAbilities;
using Enums;
using UnityEngine;

namespace Abilities.Data
{
    [CreateAssetMenu(fileName = "Movement Attack Ability Data", menuName = "ScriptableObjects/Abilities/Movement Attack Ability Data",
         order = 0), Serializable]
    public class MovementAttackAbilityData : AbilityData
    {
        [SerializeField] private float damage;

        public float Damage
        {
            get => damage;
            set => damage = value;
        }

        [SerializeField] private List<ControlType> affectedFactions;

        public List<ControlType> AffectedFactions
        {
            get => affectedFactions;
            set => affectedFactions = value;
        }
        
        [SerializeField] private float movementSpeedModifier;

        public float MovementSpeedModifier
        {
            get => movementSpeedModifier;
            set => movementSpeedModifier = value;
        }
    }
}
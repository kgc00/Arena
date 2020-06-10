using System;
using System.Collections.Generic;
using Abilities.AttackAbilities;
using Enums;
using UnityEngine;

namespace Abilities.Data
{
    [CreateAssetMenu(fileName = "Attack Ability Data", menuName = "ScriptableObjects/Abilities/Attack Ability Data",
         order = 0), Serializable]
    public class AttackAbilityData : AbilityData
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
    }
}
using System;
using System.Collections.Generic;
using Data.Types;
using UnityEngine;

namespace Data.AbilityData {
    [CreateAssetMenu(fileName = "Attack Ability Data", menuName = "ScriptableObjects/Abilities/Attack Ability Data",
         order = 0), Serializable]
    public class AttackAbilityData : AbilityData {
        [SerializeField] private List<ControlType> affectedFactions;
        [SerializeField] private float damage;

        public float Damage {
            get => damage;
            set => damage = value;
        }

        public List<ControlType> AffectedFactions {
            get => affectedFactions;
            set => affectedFactions = value;
        }
    }
}
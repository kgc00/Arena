using System;
using System.Collections.Generic;
using Data.Types;
using UnityEngine;

namespace Data.AbilityData {
    [CreateAssetMenu(fileName = "Movement Attack Ability Data",
         menuName = "ScriptableObjects/Abilities/Movement Attack Ability Data",
         order = 0), Serializable]
    public class MovementAttackAbilityData : AbilityData {
        [SerializeField] private List<ControlType> affectedFactions;
        [SerializeField] private float damage;

        [SerializeField] private float movementSpeedModifier;

        public float Damage {
            get => damage;
            set => damage = value;
        }

        public List<ControlType> AffectedFactions {
            get => affectedFactions;
            set => affectedFactions = value;
        }

        public float MovementSpeedModifier {
            get => movementSpeedModifier;
            set => movementSpeedModifier = value;
        }
    }
}
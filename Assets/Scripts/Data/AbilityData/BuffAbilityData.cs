using System;
using System.Collections.Generic;
using Data.Modifiers;
using Data.Types;
using UnityEngine;

namespace Data.AbilityData {
    [CreateAssetMenu(fileName = "Buff Ability Data", menuName = "ScriptableObjects/Abilities/Buff Ability Data",
         order = 0), Serializable]
    public class BuffAbilityData : AbilityData {
        [SerializeField] private List<ControlType> affectedFactions;

        public List<ControlType> AffectedFactions {
            get => affectedFactions;
            set => affectedFactions = value;
        }

        public float Duration {
            get => duration;
            set => duration = value;
        }
    }
}
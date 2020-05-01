using System;
using System.Collections.Generic;
using Abilities.AttackAbilities;
using Enums;
using UnityEngine;

namespace Abilities.Data
{
    [CreateAssetMenu(fileName = "Buff Ability Data", menuName = "ScriptableObjects/Abilities/Buff Ability Data",
         order = 0), Serializable]
    public class BuffAbilityData : AbilityData
    {
        [SerializeField] private List<ControlType> affectedFactions;
        [SerializeField] private float duration;

        public List<ControlType> AffectedFactions
        {
            get => affectedFactions;
            set => affectedFactions = value;
        }

        public float Duration
        {
            get => duration;
            set => duration = value;
        }
    }
}
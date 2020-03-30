using System;
using System.Collections.Generic;
using Abilities.AttackAbilities;
using Enums;
using UnityEngine;

namespace Abilities.Data
{
    [CreateAssetMenu(fileName = "Attack Ability Type", menuName = "ScriptableObjects/Abilities/Attack Ability Type", order = 0), Serializable]
    public class AttackAbilityData : AbilityData
    {
        [SerializeField] public float Damage;
        [SerializeField] public List<ControlType> AffectedFactions;
    }
}
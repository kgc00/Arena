using System;
using System.Collections.Generic;
using System.Linq;
using Abilities.Data;
using Controls;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityComponent : MonoBehaviour
    {
        public Unit Owner { get; private set; }
        [SerializeField] public Dictionary<ButtonType, Ability> equippedAbilities;
        public Ability longestRangeAbility;
        public AbilityComponent Initialize(Unit owner, List<AbilityData> abilities)
        {
            Owner = owner;
            equippedAbilities = Utils.AbilityFactory.CreateAbilitiesFromData(abilities, owner);
            longestRangeAbility = equippedAbilities.Where(a => a.Value is AttackAbility).OrderByDescending(a => a.Value.Range).First().Value;
            
            return this;
        }
    }
}
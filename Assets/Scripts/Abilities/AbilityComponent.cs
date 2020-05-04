using System;
using System.Collections.Generic;
using System.Linq;
using Abilities.Data;
using Abilities.Modifiers;
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
        
        public List<AbilityModifier> Modifiers { get; private set; } 
        public AbilityComponent Initialize(Unit owner, List<AbilityData> abilities)
        {
            Owner = owner;
         
            Modifiers = new List<AbilityModifier>();
            
            equippedAbilities = Utils.AbilityFactory.CreateAbilitiesFromData(abilities, owner);
            
            longestRangeAbility = equippedAbilities.Where(a => a.Value is AttackAbility)
                .OrderByDescending(a => a.Value.Range)
                .First().Value;
            
            return this;
        }
        
        // public void AddModifier(Modifier m, int index = -1) => Modifiers.Add(m);
        // public void RemoveModifier(Modifier m) => Modifiers.Remove(m);


        public void Activate(Ability ability, Vector3 targetLocation)
        {
            ability.ResetInstanceValues();
            
            var root = new AbilityModifier(ability);
            
            for (int i = 0; i < Modifiers.Count; i++) 
            {
                root.Add(Modifiers[i].InitializeModifier(ability));
            }

            Debug.Log($"Modifer list is {Modifiers.Count} items long");
            
            // Modifies original ability reference...
            // so we reset the values at the start of this method
            root.Handle();
            
            // clear modifiers list for next call of this function
            Modifiers.RemoveAll(m => m.ShouldConsume());

            for (int i = 0; i < ability.OnActivation.Count; i++) 
                ability.OnActivation[i](targetLocation);

            ability.Cooldown.SetOnCooldown();
        }
    }
}
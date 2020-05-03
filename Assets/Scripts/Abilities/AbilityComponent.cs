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
        
        public List<AbilityModifier> Modifiers { get; private set; } = new List<AbilityModifier>();
        // Modifiers.Add("MarkModifer");
        // logicModifier
        // public void AddModifier(Modifier m) => Modifiers.Add(m);
        // public void RemoveModifier(Modifier m) => Modifiers.Remove(m);
        public AbilityComponent Initialize(Unit owner, List<AbilityData> abilities)
        {
            Owner = owner;
            equippedAbilities = Utils.AbilityFactory.CreateAbilitiesFromData(abilities, owner);
            longestRangeAbility = equippedAbilities.Where(a => a.Value is AttackAbility).OrderByDescending(a => a.Value.Range).First().Value;
            
            return this;
        }

        public void Activate(Ability ability, Vector3 targetLocation)
        {
            // list  prey
            // uses:  1
            var root = new AbilityModifier(ability);
            
            for (int i = 0; i < Modifiers.Count; i++) // somehow creates an infinite loop when we go to handle the second time through
            {
                if (Modifiers[i] == null) continue;
                
                // if (!Modifiers[i].ShouldConsume()) toSave.Add(Modifiers[i]);
                root.Add(Modifiers[i].InitializeModifier(ability));
            }
            
            // will modify original ability... need to make a copy and run this on the copy
            root.Handle();
            
            // clear list
            
            ability.Activate(targetLocation);
            
            ability.Cooldown.SetOnCooldown();

            // Modifiers = toSave;
        }
    }
}
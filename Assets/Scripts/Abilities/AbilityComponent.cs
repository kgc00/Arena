using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.Data;
using Abilities.Modifiers;
using Controls;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace Abilities
{
    public class AbilityComponent : MonoBehaviour {
        public AbilityComponentState State { get; set; } = AbilityComponentState.NotInitialized;
        public Unit Owner { get ; private set; }
        [SerializeField] public Dictionary<ButtonType, Ability> equippedAbilities;
        public Ability longestRangeAbility;
        public List<AbilityModifier> Modifiers { get; private set; }
        private List<AbilityModifier> BuffAbilityModifiers => Modifiers.Where(x => x is BuffAbilityModifier || x.GetType() == typeof(AbilityModifier)).ToList();
        private List<AbilityModifier> AttackAbilityModifiers => Modifiers.Where(x => x is AttackAbilityModifier || x.GetType() == typeof(AbilityModifier)).ToList();

        private void UpdateState(Unit unit, Ability ability) {
            if (unit == Owner) {
                State = AbilityComponentState.Idle;
                ability.Cooldown.SetOnCooldown();
            }
        }
        
        public AbilityComponent Initialize(Unit owner, List<AbilityData> abilities)
        {
            Owner = owner;
            
            Ability.onAbilityActivationFinished += UpdateState;

            Modifiers = new List<AbilityModifier>();
            
            equippedAbilities = Utils.AbilityFactory.CreateAbilitiesFromData(abilities, owner);
            
            longestRangeAbility = equippedAbilities.Where(a => a.Value is AttackAbility)
                .OrderByDescending(a => a.Value.Range)
                .First().Value;
            
            State = AbilityComponentState.Idle;
            return this;
        }

        private void OnDisable() {
            Ability.onAbilityActivationFinished -= UpdateState;
        }

        public void Activate(Ability ability, Vector3 targetLocation) {
            State = AbilityComponentState.Executing;
            ability.ResetInstanceValues();

            var modifiers = new List<AbilityModifier>();
            
            if (ability is AttackAbility)
                modifiers = AttackAbilityModifiers;
            else if (ability is BuffAbility)
                modifiers = BuffAbilityModifiers;
            
            var root = new AbilityModifier(ability);
            
            for (int i = 0; i < modifiers.Count; i++) 
            {
                root.Add(modifiers[i].InitializeModifier(ability));
            }

            Debug.Log($"Modifer list is {modifiers.Count} items long");
            
            // Modifies original ability reference...
            // so we reset the values at the start of this method
            root.Handle();
            
            // clear modifiers list for next call of this function
            Modifiers.RemoveAll(m => m.ShouldConsume() && modifiers.Contains(m));

            StartCoroutine(ExecuteAndSetComponentState(ability, targetLocation));
        }

        private IEnumerator ExecuteAndSetComponentState(Ability ability, Vector3 targetLocation) {
            for (int i = 0; i < ability.OnActivation.Count; i++) 
                StartCoroutine(ability.OnActivation[i](targetLocation));

            yield return new WaitUntil(() => State == AbilityComponentState.Idle);
        }
    }
}
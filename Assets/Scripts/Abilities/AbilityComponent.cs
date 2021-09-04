﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.AttackAbilities;
using Abilities.Modifiers;
using Controls;
using Data.AbilityData;
using Data.Types;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using Utils.NotificationCenter;
using Object = System.Object;

namespace Abilities
{
    public class AbilityComponent : MonoBehaviour {
        public AbilityComponentState State { get; set; } = AbilityComponentState.NotInitialized;
        public Unit Owner { get ; private set; }
        [SerializeField] public Dictionary<ButtonType, Ability> equippedAbilities;
        public Ability longestRangeAbility;
        public List<AbilityModifier> GlobalAbilityModifiers { get; private set; } // used for modifiers that affect other ability's executions
        private List<AbilityModifier> BuffAbilityModifiers => GlobalAbilityModifiers.Where(x => x is BuffAbilityModifier || x.GetType() == typeof(AbilityModifier)).ToList();
        private List<AbilityModifier> AttackAbilityModifiers => GlobalAbilityModifiers.Where(x => x is AttackAbilityModifier || x.GetType() == typeof(AbilityModifier)).ToList();

        private void UpdateState(Unit unit, Ability ability) {
            if (unit == Owner) {
                State = AbilityComponentState.Idle;
                ability.Cooldown.SetOnCooldown();
            }
        }
        
        public AbilityComponent Initialize(Unit owner, List<AbilityData> abilities)
        {
            Owner = owner;
            
            Ability.OnAbilityActivationFinished += UpdateState;

            GlobalAbilityModifiers = new List<AbilityModifier>();
            
            equippedAbilities = Utils.AbilityFactory.CreateAbilitiesFromData(abilities, owner);
            
            if (equippedAbilities.Count > 0 && equippedAbilities.Values.Any((a) => a is AttackAbility))
                longestRangeAbility = equippedAbilities.Where(a => a.Value is AttackAbility)
                    .OrderByDescending(a => a.Value.Range)
                    .First().Value;
            
            State = AbilityComponentState.Idle;

            return this;
        }

        private void OnDisable() {
            Ability.OnAbilityActivationFinished -= UpdateState;
        }

        
        public void UpdateModel(List<AbilityData> abilities) {
            // TODO - confirm global ability modifiers should be reset / not reset
            Debug.Assert(State == AbilityComponentState.Idle);
            Debug.Assert(Owner != null);
            
            GlobalAbilityModifiers = new List<AbilityModifier>();
            
            equippedAbilities = Utils.AbilityFactory.CreateAbilitiesFromData(abilities, Owner);
            
            if (equippedAbilities.Count > 0 && equippedAbilities.Values.Any((a) => a is AttackAbility))
                longestRangeAbility = equippedAbilities.Where(a => a.Value is AttackAbility)
                    .OrderByDescending(a => a.Value.Range)
                    .First().Value;
        }
        
        public void Activate(ref Ability ability, Vector3 targetLocation) {
            State = AbilityComponentState.Executing;

            var modifiers = new List<AbilityModifier>();
            switch (ability) {
                case AttackAbility _:
                    modifiers = AttackAbilityModifiers;
                    break;
                case BuffAbility _:
                    modifiers = BuffAbilityModifiers;
                    break;
            }
            modifiers.AddRange(ability.Modifiers);
            
            var root = new AbilityModifier(ability);
            
            foreach (var mod in modifiers) {
                root.Add(mod.InitializeModifier(ability));
            }

            // Debug.Log($"Modifer list is {modifiers.Count} items long");
            
            // Modifies original ability reference...
            // so we reset the values at the start of this method
            root.Handle();
            
            // clear modifiers list for next call of this function
            GlobalAbilityModifiers.RemoveAll(m => m.ShouldConsume() && modifiers.Contains(m));
            ability.Modifiers.RemoveAll(m => m.ShouldConsume() && modifiers.Contains(m));

            StartCoroutine(ExecuteAndSetComponentState(ability, targetLocation));
        }

        private IEnumerator ExecuteAndSetComponentState(Ability ability, Vector3 targetLocation) {
            foreach (var effect in ability.OnActivation)
                StartCoroutine(effect(targetLocation));

            yield return new WaitUntil(() => State == AbilityComponentState.Idle);
            /*
             * MAY introduce bugs due to concurrent ability executions but probably fine
             * but here because we need to remove old modifiers after ability execution. 
             */
            ability.ResetInstanceValuesExcludingSpentModifiers(); 
        }

        [CanBeNull]
        public TAbility GetEquippedAbility<TAbility>() where TAbility : Ability =>
            (TAbility) equippedAbilities.Values.FirstOrDefault(a => a.GetType() == typeof(TAbility)) 
                            ?? throw new Exception($"{typeof(TAbility)} ability was not found by {Owner}'s {this}");
    }
}
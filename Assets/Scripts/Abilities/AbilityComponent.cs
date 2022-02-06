using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.Modifiers;
using Components;
using Data.AbilityData;
using Data.Types;
using JetBrains.Annotations;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityComponent : MonoBehaviour {
        private StatsComponent _statsComponent;
        public AbilityComponentState State { get; set; } = AbilityComponentState.NotInitialized;
        public Unit Owner { get ; private set; }
        [SerializeField] public Dictionary<ButtonType, Ability> equippedAbilitiesByButton;
        public Dictionary<AbilityType, Ability> equippedAbilitiesByType;
        public Ability longestRangeAbility;
        public List<AbilityModifier> GlobalAbilityModifiers { get; private set; } // used for modifiers that affect other ability's executions
        private List<AbilityModifier> BuffAbilityModifiers => GlobalAbilityModifiers.Where(x => x is BuffAbilityModifier || x.GetType() == typeof(AbilityModifier)).ToList();
        private List<AbilityModifier> AttackAbilityModifiers => GlobalAbilityModifiers.Where(x => x is AttackAbilityModifier || x.GetType() == typeof(AbilityModifier)).ToList();
        public void SetAbilityComponentOnCooldown() {
                State = AbilityComponentState.Idle;
        }
        
        public AbilityComponent Initialize(Unit owner, List<AbilityData> abilities, StatsComponent statsComponent) {
            Owner = owner;
            _statsComponent = statsComponent;
            GlobalAbilityModifiers = new List<AbilityModifier>();
            equippedAbilitiesByButton = Utils.AbilityFactory.CreateAbilitiesFromData(abilities, owner, _statsComponent);
            CreateEquippedAbilitiesByType(equippedAbilitiesByButton);
            if (equippedAbilitiesByButton.Count > 0 && equippedAbilitiesByButton.Values.Any((a) => a is AttackAbility))
                longestRangeAbility = equippedAbilitiesByButton.Where(a => a.Value is AttackAbility)
                    .OrderByDescending(a => a.Value.Range)
                    .First().Value;

            State = AbilityComponentState.Idle;
            return this;
        }

        public void UpdateModel(List<AbilityData> abilities) {
            // TODO - confirm global ability modifiers should be reset / not reset
            Debug.Assert(State == AbilityComponentState.Idle);
            Debug.Assert(Owner != null);
            
            GlobalAbilityModifiers = new List<AbilityModifier>();

            if (equippedAbilitiesByType == null) equippedAbilitiesByType = new Dictionary<AbilityType, Ability>();
            equippedAbilitiesByButton = Utils.AbilityFactory.CreateAbilitiesFromData(abilities, Owner, _statsComponent);
            CreateEquippedAbilitiesByType(equippedAbilitiesByButton);
            
            if (equippedAbilitiesByButton.Count > 0 && equippedAbilitiesByButton.Values.Any((a) => a is AttackAbility))
                longestRangeAbility = equippedAbilitiesByButton.Where(a => a.Value is AttackAbility)
                    .OrderByDescending(a => a.Value.Range)
                    .First().Value;
        }
        
        public Coroutine Activate(ref Ability ability, Vector3 targetLocation) {
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

            return StartCoroutine(ExecuteAndSetComponentState(ability, targetLocation));
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

        private void CreateEquippedAbilitiesByType(Dictionary<ButtonType, Ability> initializedAbilities) {
            if (equippedAbilitiesByType == null) equippedAbilitiesByType = new Dictionary<AbilityType, Ability>();
            equippedAbilitiesByType = initializedAbilities.Values.ToDictionary((a) => a.Type);
        }

        [CanBeNull]
        public TAbility GetEquippedAbility<TAbility>() where TAbility : Ability =>
            (TAbility) equippedAbilitiesByType.Values.FirstOrDefault(a => a.GetType() == typeof(TAbility));
    }
}
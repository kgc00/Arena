using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.Modifiers;
using Abilities.Modifiers.AbilityModifierShopData;
using Components;
using Data.AbilityData;
using Data.Modifiers;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Abilities {
    public class Intent { }

    public abstract class Ability : MonoBehaviour {
        protected StatsComponent StatsComponent;
        public AbilityType Type { get; protected set; }
        public AbilityData Model { get; private set; }
        public float EnergyCost { get; protected set; }
        public string Description { get; protected set; }
        public string DisplayName { get; protected set; }
        public float Range { get; protected set; }
        public float Force { get; protected set; }
        public float ProjectileSpeed { get; protected set; }
        public float Duration { get; protected set; }
        public IndicatorType IndicatorType { get; protected set; }
        public float AreaOfEffectRadius { get; protected set; }
        public float StartupTime { get; protected set; }
        public Cooldown Cooldown { get; protected set; }
        public Unit Owner { get; protected set; }
        public Sprite Icon { get; protected set; }        
        public bool Initialized { get; private set; }
        public List<AbilityModifier> Modifiers { get; protected set; }
        public static Action<Unit, Ability> OnAbilityActivationFinished { get; set; } = delegate { };
        public List<Func<Vector3, IEnumerator>> OnActivation { get; set; }
        public abstract IEnumerator AbilityActivated(Vector3 targetLocation);

        public List<Action<Unit, Ability>> OnAbilityFinished { get; set; }

        public List<AbilityModifierType> EquipableModifiers { get; protected set; }

        public List<AbilityModifierShopData> EquipableModifiersShopData { get; protected set; }
        // public List<Func<Vector3, IEnumerator>> OnAoEExecution { get; set; }
        // public abstract IEnumerator AbilityAoEExecuted(Vector3 targetLocation);

        protected virtual void LateUpdate() => Cooldown.UpdateCooldown(Time.deltaTime);

        protected virtual Ability Initialize(AbilityData data, Unit owner, StatsComponent statsComponent) {
            Owner = owner;
            Model = data;
            StatsComponent = statsComponent;
            Range = data.range;
            Force = data.force;
            Icon = data.icon;
            Duration = data.duration;
            AreaOfEffectRadius = StatsComponent.GetAoERadius(data.areaOfEffectRadius);
            IndicatorType = data.indicatorType;
            var currentTimeLeft = Cooldown?.TimeLeft ?? Cooldown.DefaultTimeLeft;
            var cooldownIsFrozen = Cooldown?.IsFrozen ?? default;
            var reducedCooldown = StatsComponent.GetAbilityCooldown( data.cooldown,data.minimumCooldown);
            Cooldown = new Cooldown(reducedCooldown, currentTimeLeft, cooldownIsFrozen);
            StartupTime = data.startupTime;
            ProjectileSpeed = data.projectileSpeed;
            OnActivation = new List<Func<Vector3, IEnumerator>> {AbilityActivated};
            OnAbilityFinished = new List<Action<Unit, Ability>> { };
            DisplayName = data.displayName;
            Description = data.description;
            EnergyCost = data.energyCost;
            Modifiers = new List<AbilityModifier>();
            EquipableModifiers = data.equipableModifiers;
            EquipableModifiersShopData = EquipableModifiers
                .Select(AbilityFactory.AbilityModifierShopDataFromType)
                .ToList();
            Type = data.type;
            Initialized = true;
            return this;
        }

        protected void ExecuteOnAbilityFinished() {
            Cooldown.SetOnCooldown();
            Owner.AbilityComponent.SetAbilityComponentOnCooldown();
            foreach (var cb in OnAbilityFinished) {
                cb(Owner, this);
            }
        }


        public void AddModifier(AbilityModifierType modifierType) {
            if (Modifiers.Exists(x => x.Type == modifierType) 
                || !EquipableModifiers.Contains(modifierType)) {
                return;
            }
            
            // Model.modifiers.Add(modifierType); // if we want to permanently modify the spawn data
            Modifiers.Add(AbilityFactory.AbilityModifierFromEnum(this, modifierType));
        }
        
        public abstract void ResetInstanceValuesExcludingSpentModifiers();
    }
}
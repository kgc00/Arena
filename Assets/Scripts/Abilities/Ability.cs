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
        public AbilityTargetLocationSelectionType TargetLocationSelectionType { get; protected set; }
        public AbilityData Model { get; private set; }
        public int UnlockCost { get; protected set; }
        public bool Unlocked { get; protected set; }
        public float EnergyCost { get; protected set; }
        public string Description { get; protected set; }
        public string DisplayName { get; protected set; }
        public float Range { get; protected set; }
        public float Force { get; protected set; }
        public float ProjectileSpeed { get; protected set; }
        public float Duration { get; protected set; }
        public IndicatorType IndicatorType { get; protected set; }
        // todo maybe define a AoE Targeting Data class
        public float AreaOfEffectCircularRadius { get; protected set; }
        public float AreaOfEffectRectangularWidth { get; protected set; }
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
            UnlockCost = data.unlockCost;
            Unlocked = data.unlocked;
            Range = data.range;
            Force = data.force;
            Icon = data.icon;
            Duration = data.duration;
            AreaOfEffectCircularRadius = StatHelpers.GetAoERadius(data.areaOfEffectCircularRadius, statsComponent.Stats);
            AreaOfEffectRectangularWidth = StatHelpers.GetAoERadius(data.areaOfEffectRectangularWidth, statsComponent.Stats);
            IndicatorType = data.indicatorType;
            var currentTimeLeft = Cooldown?.TimeLeft ?? Cooldown.DefaultTimeLeft;
            var cooldownIsFrozen = Cooldown?.IsFrozen ?? default;
            var reducedCooldown = StatHelpers.GetAbilityCooldown(data.cooldown, statsComponent.Stats, data.minimumCooldown);
            Cooldown = new Cooldown(reducedCooldown, currentTimeLeft, cooldownIsFrozen);
            StartupTime = data.startupTime;
            ProjectileSpeed = data.projectileSpeed;
            OnActivation = new List<Func<Vector3, IEnumerator>> { AbilityActivated };
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
            TargetLocationSelectionType = data.targetLocationSelectionType;
            Initialized = true;
            return this;
        }

        /**
        * Hook to allow StateSkillBehavior and TargetingUIController a way to 
        * override the target destination for an ability. Used primarily to clamp
        * the targeting ui and target deestination for a skill with a certain range
        * of the caster -- see overrides.
        */
        public virtual Vector3 GetFinalizedTargetLocation(Vector3 targetLocation) {
            var ownerPosition = Owner.transform.position;
            var heading = targetLocation - ownerPosition;
            bool isBeyondMaxRange = Vector3.Distance(ownerPosition, targetLocation) > Range;
            switch (TargetLocationSelectionType) {
                case AbilityTargetLocationSelectionType.MAX_RANGE:
                    return ownerPosition + (heading.normalized * Range);
                case AbilityTargetLocationSelectionType.SELECTED_LOCATION:
                    return targetLocation;
                case AbilityTargetLocationSelectionType.SELECTED_LOCATION_CAPPED_BY_MAX_RANGE:
                    return isBeyondMaxRange ? ownerPosition + (heading.normalized * Range) : targetLocation;
            }
            throw new Exception("Unable to provide a target location for AbilityTargetLocationSelectionType of type " + TargetLocationSelectionType);
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

        // if we want to clear modifiers obtained during the course of the game, we can add a new method
        public abstract void ReinitializeDataWhileRetainingNewModifiers();
    }
}
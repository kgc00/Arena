using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Modifiers;
using Data.AbilityData;
using Data.Modifiers;
using Data.Types;
using Units;
using UnityEngine;

namespace Abilities {
    public class Intent { }

    public abstract class Ability : MonoBehaviour {
        public AbilityData Model { get; private set; }
        public float EnergyCost { get; protected set; }
        public string Description { get; protected set; }
        public string DisplayName { get; protected set; }
        public float Range { get; protected set; }
        public float Force { get; protected set; }
        public float ProjectileSpeed { get; protected set; }
        public float Duration { get; protected set; }
        public IndicatorType IndicatorType { get; protected set; }
        public int AreaOfEffectRadius { get; protected set; }
        public float StartupTime { get; protected set; }
        public Cooldown Cooldown { get; protected set; }
        public Unit Owner { get; protected set; }
        public Sprite Icon { get; protected set; }
        public List<AbilityModifier> Modifiers { get; protected set; }
        public static Action<Unit, Ability> OnAbilityActivationFinished { get; set; } = delegate { };
        public List<Func<Vector3, IEnumerator>> OnActivation { get; set; }
        public abstract IEnumerator AbilityActivated(Vector3 targetLocation);

        public List<Action<Unit, Ability>> OnAbilityFinished { get; set; }

        public List<AbilityModifierType> EquipableModifiers { get; protected set; }
        // public List<Func<Vector3, IEnumerator>> OnAoEExecution { get; set; }
        // public abstract IEnumerator AbilityAoEExecuted(Vector3 targetLocation);

        protected virtual void LateUpdate() => Cooldown.UpdateCooldown(Time.deltaTime);

        protected virtual Ability Initialize(AbilityData data, Unit owner) {
            Owner = owner;
            Model = data;
            Range = data.range;
            Force = data.force;
            Icon = data.icon;
            Duration = data.duration;
            AreaOfEffectRadius = data.areaOfEffectRadius;
            IndicatorType = data.indicatorType;
            Cooldown = new Cooldown(data.cooldown);
            StartupTime = data.startupTime;
            ProjectileSpeed = data.projectileSpeed;
            OnActivation = new List<Func<Vector3, IEnumerator>> { AbilityActivated };
            OnAbilityFinished = new List<Action<Unit, Ability>> {};
            DisplayName = data.displayName;
            Description = data.description;
            EnergyCost = data.energyCost;
            Modifiers = new List<AbilityModifier>();
            EquipableModifiers = data.equipableModifiers;
            return this;
        }

        public abstract void ResetInstanceValuesExcludingSpentModifiers();
    }
}
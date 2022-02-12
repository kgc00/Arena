using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.AttackAbilities;
using Abilities.Modifiers;
using Components;
using Data.AbilityData;
using Data.Types;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class AttackAbility : Ability, IDamageDealer
    {
        private new AttackAbilityData Model { get; set; }
        public float Damage { get; set;}
        public List<ControlType> AffectedFactions { get; private set; }
        public List<Action<GameObject, GameObject>> OnAbilityConnection { get; set; }
        protected abstract void AbilityConnected(GameObject target, GameObject projectile = null);

        public virtual AttackAbility Initialize(AttackAbilityData data, Unit owner, StatsComponent statsComponent) {
            base.Initialize(data, owner, statsComponent);
            Model = data;
            Damage = Utils.StatHelpers.GetDamage(data.Damage, statsComponent.Stats);
            AffectedFactions = data.AffectedFactions;
            OnAbilityConnection = new List<Action<GameObject, GameObject>>() {AbilityConnected};
            Modifiers = new List<AbilityModifier>();
            data.modifiers.ForEach(type => Modifiers.Add(Utils.AbilityFactory.AbilityModifierFromEnum(this, type)));
            return this;
        }

        public override void ResetInstanceValuesExcludingSpentModifiers()
        {
            if (Model == null || Owner == null)
            {
                Debug.Log("Model or Owner are not set and we cannot reset instance values");
                return;
            }

            var previousMods = Modifiers;
            Initialize(Model, Owner, StatsComponent);
            Modifiers = previousMods.Intersect(Modifiers).ToList();
        }

        public override string ToString()
        {
            return string.Format($"Attack ability has values: Owner: {Owner}" +
            "Model: {Model}"+
            "Damage: {Damage}"+
            "Range: {Range}"+
            "AreaOfEffectRadius: {AreaOfEffectRadius}"+
            "AffectedFactions: {AffectedFactions}"+
            "Cooldown: {Cooldown}"+
            "StartupTime: {StartupTime}"+
            "IndicatorType: {IndicatorType}"+
            "OnActivation: {OnActivation}"+
            "OnAbilityConnected: {OnAbilityConnected}"
            );
        }
    }
}
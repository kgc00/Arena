using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.AttackAbilities;
using Abilities.Data;
using Enums;
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

        public virtual AttackAbility Initialize(AttackAbilityData data, Unit owner) {
            base.Initialize(data, owner);
            Model = data;
            Damage = data.Damage;
            AffectedFactions = data.AffectedFactions;
            OnAbilityConnection = new List<Action<GameObject, GameObject>>() {AbilityConnected};
            return this;
        }

        public override void ResetInstanceValues()
        {
            if (Model == null || Owner == null)
            {
                Debug.Log("Model or Owner are not set and we cannot reset instance values");
                return;
            }

            Initialize(Model, Owner);
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
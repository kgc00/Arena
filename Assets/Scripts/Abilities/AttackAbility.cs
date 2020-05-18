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
        public new AttackAbilityData Model { get; private set; }
        public float Damage { get; set;}
        public List<ControlType> AffectedFactions { get; private set; }
        public List<Action<GameObject, GameObject>> OnAbilityConnection { get; set; }
        public abstract void AbilityConnected(GameObject target, GameObject projectile = null);

        public AttackAbility Initialize(AttackAbilityData data, Unit owner)
        {
            Owner = owner;
            Model = data;
            Damage = data.Damage;
            Range = data.range;
            Force = data.force;
            AreaOfEffectRadius = data.areaOfEffectRadius;
            AffectedFactions = data.AffectedFactions;
            Cooldown = new Cooldown(data.cooldown);
            StartupTime = data.startupTime;
            ProjectileSpeed = data.projectileSpeed;
            IndicatorType = data.indicatorType;
            OnActivation = new List<Func<Vector3, IEnumerator>>() {AbilityActivated};
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
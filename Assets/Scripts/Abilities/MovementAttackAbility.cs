using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using Data.AbilityData;
using Data.Types;
using Units;
using UnityEngine;

namespace Abilities {
    public abstract class MovementAttackAbility : Ability, IDamageDealer, IMovementUser {
        private new MovementAttackAbilityData Model { get; set; }
        public float MovementSpeedModifier { get; set; }

        public Action<Unit, Ability> DestinationReached { get; set; }
        public List<Action<Unit, Ability>> OnDestinationReached { get; set; }

        // public float MaxTravelDistance { get; set; }
        public float Damage { get; set;}
        public List<ControlType> AffectedFactions { get; private set; }
        public List<Action<GameObject, GameObject>> OnAbilityConnection { get; set; }
        protected abstract void AbilityConnected(GameObject target, GameObject projectile = null);

        public virtual MovementAttackAbility Initialize(MovementAttackAbilityData data, Unit owner,
            StatsComponent statsComponent) {
            base.Initialize(data, owner, statsComponent);
            Model = data;
            Damage = StatsComponent.GetDamage(data.Damage);
            MovementSpeedModifier = data.MovementSpeedModifier;
            AffectedFactions = data.AffectedFactions;
            OnAbilityConnection = new List<Action<GameObject, GameObject>> { AbilityConnected };
            OnDestinationReached = new List<Action<Unit, Ability>> { DestinationReached };
            return this;
        }

        public override void ResetInstanceValuesExcludingSpentModifiers()
        {
            if (Model == null || Owner == null)
            {
                Debug.Log("Model or Owner are not set and we cannot reset instance values");
                return;
            }

            Initialize(Model, Owner, StatsComponent);
        }

        public override string ToString() => string.Format($"Movement Attack Ability {DisplayName} is equipped by {Owner}");
    }
}
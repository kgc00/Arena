using System;
using System.Collections;
using System.Collections.Generic;
using Data.AbilityData;
using Data.Types;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class BuffAbility : Ability, IBuffUser
    {
        private new BuffAbilityData Model { get; set; }
        public List<ControlType> AffectedFactions { get; protected set; }
        public virtual BuffAbility Initialize(BuffAbilityData data, Unit owner) {
            base.Initialize(data, owner);
            Model = data;
            AffectedFactions = data.AffectedFactions;
            OnActivation = new List<Func<Vector3, IEnumerator>> {targetLocation => AbilityActivated(targetLocation)};
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
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Modifiers;
using Components;
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
        public virtual BuffAbility Initialize(BuffAbilityData data, Unit owner, StatsComponent statsComponent) {
            base.Initialize(data, owner, statsComponent);
            Model = data;
            AffectedFactions = data.AffectedFactions;
            OnActivation = new List<Func<Vector3, IEnumerator>> {AbilityActivated};
            data.modifiers.ForEach(type => Modifiers.Add(Utils.AbilityFactory.AbilityModifierFromEnum(this, type) as BuffAbilityModifier));
            return this;
        }
        
        public override void ReinitializeDataWhileRetainingNewModifiers()
        {
            if (Model == null || Owner == null)
            {
                Debug.Log("Model or Owner are not set and we cannot reset instance values");
                return;
            }
            
            var previousMods = Modifiers;
            Initialize(Model, Owner, StatsComponent);
            Modifiers = previousMods;
        }
    }
}
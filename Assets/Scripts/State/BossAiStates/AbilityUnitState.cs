﻿using System;
using System.Collections;
using Abilities;
using Controls;
using Data.Types;
using Units;
using UnityEngine;

namespace State.BossAiStates {
    public abstract class AbilityUnitState<T> : BossState where T : Ability {
        protected bool abilityFinished;
        protected Ability ability;

        protected AbilityUnitState(Unit owner) : base(owner) {
            ability = Owner.AbilityComponent.GetEquippedAbility<T>();
            if (ability == null) {
                throw new Exception("Ability must be assigned in AbilityUnitState constructor");
            }
            abilityFinished = false;
            if (!ability.OnAbilityFinished.Contains(HandleAbilityFinished)) {
                ability.OnAbilityFinished.Insert(0, HandleAbilityFinished);
            }
        }

        public override void Exit() {
            // if (ability.OnAbilityFinished.Contains(HandleAbilityFinished)) {
            //     ability.OnAbilityFinished.Remove(HandleAbilityFinished);
            // }
        }
        
        public override void Enter() => Owner.CoroutineHelper.SpawnCoroutine(HandleAbility());

        protected virtual void HandleAbilityFinished(Unit u, Ability a) {
            if (u != Owner || a != ability) return;

            abilityFinished = true;
        }

        protected abstract IEnumerator HandleAbility();
    }
}
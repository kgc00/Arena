﻿using System.Collections;
using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;

namespace State.BossAiStates {
    public class ChainFlameUnitState : AbilityUnitState<ChainFlame> {
        private readonly Transform playerTransform;
        private static readonly int ChainFlame = Animator.StringToHash("ChainFlame");

        public ChainFlameUnitState(Unit owner, Transform playerTransform) : base(owner) => this.playerTransform = playerTransform;

        public override void Exit() {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(ChainFlame);
        }
        protected override IEnumerator HandleAbility() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) {
                abilityFinished = true;
                yield break;
            }
            Owner.Animator.SetTrigger(ChainFlame);
            Owner.AbilityComponent.Activate(ref ability, playerTransform.position);
        }

        public override UnitState HandleUpdate(InputValues input) {
            if (abilityFinished != true) return null;
            return new IdleUnitState(Owner);
        }
        
        public override void HandleFixedUpdate(InputValues input) {
            if (playerTransform == null) return;
            UpdateUnitRotation();
        }

        private void UpdateUnitRotation()
        {
            var difference = playerTransform.position - Owner.transform.position;
            Owner.Rigidbody.MoveRotation(Quaternion.LookRotation(difference));
        }
    }
}
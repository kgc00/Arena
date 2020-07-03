using System;
using System.Collections;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;

namespace State.ChargingAiStates {
    public class ChargeUnitState : UnitState {
        private static readonly int Charging = Animator.StringToHash("Charging");
        private readonly Transform playerTransform;
        private MovementAttackAbility charge;
        private bool charging = false;
        public ChargeUnitState(Unit owner, Transform playerTransform) : base(owner) {
            this.playerTransform = playerTransform;
            charge = owner.AbilityComponent.GetEquippedAbility<Charge>();
            Charge.OnAbilityFinished += HandleChargeFinished;
            charging = true;
        }

        void HandleChargeFinished(Unit u, Ability a) {
            if (u == Owner && a == charge) charging = false;
        }

        public override void Enter() => Owner.CoroutineHelper.SpawnCoroutine(HandleCharge());

        private IEnumerator HandleCharge()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) yield break;
            Owner.Animator.SetTrigger(Charging);
            
            Owner.AbilityComponent.Activate(charge, playerTransform.position);
            
            yield return new WaitUntil(() => !charging);
        } 
        
        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Charging);
        }

        public override UnitState HandleUpdate(InputValues input) {
            if (charging) return null;
            return new IdleUnitState(Owner);
        }
    }
}
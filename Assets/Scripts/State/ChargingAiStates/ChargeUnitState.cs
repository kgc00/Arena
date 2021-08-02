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
        private Charge charge;
        private bool charging;
        private bool impactedWall;
        Ability chargeAsAbility;
        public ChargeUnitState(Unit owner, Transform playerTransform) : base(owner) {
            this.playerTransform = playerTransform;
            charge = owner.AbilityComponent.GetEquippedAbility<Charge>();
            chargeAsAbility = charge;
            Ability.OnAbilityFinished += HandleChargeFinished;
            charging = true;
        }

        ~ChargeUnitState() => Ability.OnAbilityFinished -= HandleChargeFinished;

        void HandleChargeFinished(Unit u, Ability a) {
            if (u != Owner || a != charge) return;
            
            charging = false;
            impactedWall = charge.ImpactedWall;
        }

        public override void Enter() => Owner.CoroutineHelper.SpawnCoroutine(HandleCharge());

        private IEnumerator HandleCharge() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) yield break;
            Owner.Animator.SetTrigger(Charging);

            var targetPosition = GetTargetPosition(playerTransform.position);
            Owner.AbilityComponent.Activate(ref chargeAsAbility, targetPosition);
            
            yield return new WaitUntil(() => !charging);
        }

        /// <summary>
        /// Calculates target position as 5 'units' behind the target gameobject
        /// </summary>
        /// <param name="position"></param>
        /// <returns>targetPosition</returns>
        private Vector3 GetTargetPosition(Vector3 position) {
            var heading = position - Owner.transform.position;
            heading.y = 0;
            var offset = heading * 5f;
            var targetPosition = position + offset;
            return targetPosition;
        }

        public override void Exit() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Charging);
        }

        public override UnitState HandleUpdate(InputValues input) {
            if (charging) return null;
            if (impactedWall) return new StunUnitState(Owner, charge.WallImpactStunDuration);
            return new IdleUnitState(Owner);
        }
    }
}
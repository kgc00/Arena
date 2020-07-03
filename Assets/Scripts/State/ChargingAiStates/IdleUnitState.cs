using System.Linq;
using Abilities.AttackAbilities;
using Controls;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using Utils;

namespace State.ChargingAiStates {
    public class IdleUnitState : UnitState {
        private Transform playerTransform;
        private readonly float attackRange;
        private static readonly int Idle = Animator.StringToHash("Idle");

        public IdleUnitState(Unit owner) : base(owner) =>
            attackRange = Owner.AbilityComponent.longestRangeAbility.Range;

        public override void Enter() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.SetTrigger(Idle);
        }

        public override void Exit() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Idle);
        }

        public override UnitState HandleUpdate(InputValues input) {
            // TODO: add some leashing mechanic or vision limiter

            if (playerTransform == null) {
                playerTransform = Locator.GetClosestVisiblePlayerUnit(Owner.transform.position);
                return null;
            }

            var dist = Vector3.Distance(playerTransform.position, Owner.transform.position);

            if (ShouldEnterCharge(out var unitState, dist)) return unitState;
            if (ShouldEnterAttack(out unitState, dist)) return unitState;

            return new ChaseUnitState(Owner, playerTransform);
        }

        private bool ShouldEnterAttack(out UnitState unitState, float dist) {
            unitState = null;

            // are we close enough away to use attack?
            if (dist > attackRange) return false;

            unitState = new AttackUnitState(Owner, playerTransform);
            return true;
        }

        private bool ShouldEnterCharge([CanBeNull] out UnitState unitState, float dist) {
            unitState = null;

            // find charge ability
            var charge = Owner.AbilityComponent.equippedAbilities.Values.FirstOrDefault(a => a is Charge);
            if (charge == null) return false;

            // is charge ready to use?
            if (charge.Cooldown.IsOnCooldown) return false;

            // are we too close to use charge?
            if (dist <= attackRange) return false;

            unitState = new ChargeUnitState(Owner, playerTransform);
            return true;
        }
    }
}
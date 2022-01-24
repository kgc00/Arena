using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.ChargingAiStates {
    public class IdleUnitState : UnitState {
        private Transform playerTransform;
        private Charge charge;
        private BodySlam bodySlam;
        private readonly float attackRange;
        private static readonly int Idle = Animator.StringToHash("Idle");

        public IdleUnitState(Unit owner) : base(owner) {
            Debug.Log(Owner.AbilityComponent.equippedAbilitiesByButton.Values.Count);
            Debug.Log(Owner.AbilityComponent.equippedAbilitiesByType.Values.Count);
            charge = Owner.AbilityComponent.GetEquippedAbility<Charge>();
            bodySlam = Owner.AbilityComponent.GetEquippedAbility<BodySlam>();
        }

        public override void Enter() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.SetTrigger(Idle);
        }

        public override void Exit() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Idle);
        }

        public override UnitState HandleUpdate(InputValues input) {
            UnitState nextState = null;

            if (playerTransform == null) {
                playerTransform = Locator.GetClosestVisiblePlayerUnit(Owner.transform.position);
                return nextState;
            }

            var dist = Vector3.Distance(playerTransform.position, Owner.transform.position);

            if (ShouldEnterAttack(ref nextState, dist)) return nextState;
            if (ShouldEnterCharge(ref nextState, dist)) return nextState;

            return new ChaseUnitState(Owner, playerTransform);
        }

        private bool ShouldEnterAttack(ref UnitState unitState, float dist) {
            var abilityWillNotReach = dist > bodySlam.Range;
            var abilityCoolingDown = bodySlam.Cooldown.IsOnCooldown;
            if (abilityWillNotReach || abilityCoolingDown) return false;

            unitState = new BodySlamState(Owner, playerTransform);
            return true;
        }

        private bool ShouldEnterCharge(ref UnitState unitState, float dist) {
            var abilityWillNotReach = dist > charge.Range;
            var abilityCoolingDown = charge.Cooldown.IsOnCooldown;
            if (abilityWillNotReach || abilityCoolingDown) return false;

            unitState = new ChargeUnitState(Owner, playerTransform);
            return true;
        }
    }
}
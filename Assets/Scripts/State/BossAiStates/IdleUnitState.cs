using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.BossAiStates
{
    public class IdleUnitState : BossState
    {
        private Transform playerTransform;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private Roar roar;

        public IdleUnitState(Unit owner) : base(owner) {
            roar = Owner.AbilityComponent.GetEquippedAbility<Roar>();
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.SetTrigger(Idle);
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Idle);
        }

        public override UnitState HandleUpdate(InputValues input) {
            // TODO: add some leashing mechanic or vision limiter
            if (playerTransform == null) playerTransform = Locator.GetClosestVisiblePlayerUnit(Owner.transform.position);

            if (playerTransform == null) return null;
            
            var dist = Vector3.Distance(playerTransform.position, Owner.transform.position);

            if (ShouldEnterRelocateState(out var unitState, dist)) return unitState;
            if (ShouldEnterShield(out unitState, dist)) return unitState;
            return null;
        }

        private bool ShouldEnterShield(out UnitState unitState, float dist) {
            unitState = null;

            if (!playerTransform.GetComponent<Unit>().StatusComponent.IsVisible()) return false;

            if (dist >= roar.Range) return false;
            
            unitState = new MagicShieldUnitState(Owner);
            return true;
        }

        private bool ShouldEnterRelocateState(out UnitState unitState, float dist) {
            unitState = null;

            if (!playerTransform.GetComponent<Unit>().StatusComponent.IsVisible()) return false;

            if (dist < roar.Range) return false;
            
            unitState = new RelocateUnitState(Owner, playerTransform);
            return true;
        }
    }
}
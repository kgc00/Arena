using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.BossAiStates
{
    public class IdleUnitState : UnitState
    {
        private Transform playerTransform;
        private static readonly int Idle = Animator.StringToHash("Idle");

        public IdleUnitState(Unit owner) : base(owner) { }

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

            return new MagicShieldUnitState(Owner);
            
            var dist = Vector3.Distance(playerTransform.position, Owner.transform.position);

            if (ShouldEnterLongRangeState(out var unitState, dist)) return unitState;
            if (ShouldEnterCloseRangeState(out unitState, dist)) return unitState;
            return null;
        }

        private bool ShouldEnterCloseRangeState(out UnitState unitState, float dist) {
            unitState = null;

            if (!playerTransform.GetComponent<Unit>().StatusComponent.IsVisible()) return false;

            if (dist >= 4) return false;
            
            unitState = new CloseRangeUnitState(Owner);
            return true;
        }

        private bool ShouldEnterLongRangeState(out UnitState unitState, float dist) {
            unitState = null;

            if (!playerTransform.GetComponent<Unit>().StatusComponent.IsVisible()) return false;

            if (dist < 4) return false;
            
            unitState = new LongRangeUnitState(Owner);
            return true;
        }
    }
}
using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.RangedAiStates
{
    public class IdleUnitState : UnitState
    {
        private Transform playerTransform;
        private static readonly int Idle = Animator.StringToHash("Idle");

        public IdleUnitState(Unit owner) : base(owner) {
            playerTransform = Locator.GetClosestVisiblePlayerUnit(Owner.transform.position);
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

        public override UnitState HandleUpdate(InputValues input)
        {
            // TODO: add some leashing mechanic or vision limiter
            if (playerTransform != null) return new ChaseUnitState(Owner, playerTransform);
            
            playerTransform = Locator.GetClosestVisiblePlayerUnit(Owner.transform.position);
            return playerTransform == null ? null : new ChaseUnitState(Owner, playerTransform);
        }
    }
}
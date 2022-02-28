using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.BombThrowingAiStates
{
    public class IdleUnitState : BombThrowingAiState
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
            var isStunned = base.HandleUpdate(input);
            if (isStunned != null) return isStunned;
            
            if (playerTransform != null) return new RelocateUnitState(Owner, playerTransform);
            
            playerTransform = Locator.GetClosestVisiblePlayerUnit(Owner.transform.position);
            return playerTransform == null ? null : new RelocateUnitState(Owner, playerTransform);
        }
    }
}
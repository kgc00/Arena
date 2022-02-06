using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.MeleeAiStates
{
    public class IdleUnitState : MeleeAiState
    {
        private readonly float movementSpeed = 2f;
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

        public override UnitState HandleUpdate(InputValues input)
        {
            var isStunned = base.HandleUpdate(input);
            if (isStunned != null) return isStunned;
            
            // TODO: add some leashing mechanic or vision limiter
            if (playerTransform == null) playerTransform = Locator.GetClosestVisiblePlayerUnit(Owner.transform.position);

            if (playerTransform == null) return null;
            
            return new ChaseUnitState(Owner, playerTransform);
        }
    }
}